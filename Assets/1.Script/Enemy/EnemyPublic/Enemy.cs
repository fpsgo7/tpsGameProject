using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : LivingEntity
{
    private enum State
    {
        Patrol,
        Tracking,
        AttackBegin,
        Attacking
    }

    private State state;

    public NavMeshAgent agent; // 경로계산 AI 에이전트
    private Animator animator; // 애니메이터 컴포넌트

    public Transform attackRoot;
    public Transform eyeTransform;

    private AudioSource audioPlayer; // 오디오 소스 컴포넌트
    public AudioClip hitClip; // 피격시 재생할 소리
    public AudioClip deathClip; // 사망시 재생할 소리

    private Renderer skinRenderer; // 렌더러 컴포넌트

    public float runSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;//회전 지연속도
    private float turnSmoothVelocity;//좀비회전에 실시간 변화량

    public float enemyHealth;// 체력 확인용
    public float damage = 30f;
    public float attackRadius = 2f;
    private float attackDistance;

    public float fieldOfView = 50f;
    public float viewDistance = 10f;
    public float patrolSpeed = 3f;

    public LivingEntity targetEntity; // 추적할 대상
    public LayerMask whatIsTarget; // 추적 대상 레이어

    //적의 공격을 범위 기반이라서 여러 개의 충돌포인트가 생긴다.
    private RaycastHit[] hits = new RaycastHit[10];
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();
    // 람다식을 활용한다.
    //targetEntity 가 널이아니고 추적할 대상이 죽지 않았다면  true 가 된다.
    private bool isHasTarget => targetEntity != null && !targetEntity.IsDead;


#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        //좀비의 시야와 공격범위를 그려준다.
        if (attackRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(attackRoot.position, attackRadius);
        }

        var leftRayRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        var leftRayDirection = leftRayRotation * transform.forward;
        Handles.color = new Color(1f, 1f, 1f, 0.2f);
        Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
    }

#endif

    private void Awake()
    {
        //컴포넌트 연결
        agent = GetComponent<NavMeshAgent>();//네비게이션 연결
        animator = GetComponent<Animator>();//에니메이터 연결
        audioPlayer = GetComponent<AudioSource>();//오디오 연결
        skinRenderer = GetComponentInChildren<Renderer>();//렌더러 연결

        //공격 거리
        attackDistance = Vector3.Distance(transform.position,
                             new Vector3(attackRoot.position.x, transform.position.y, attackRoot.position.z)) +
                         attackRadius;

        attackDistance += agent.radius;
        //네비게이션 에이전트의 값 초기화
        agent.stoppingDistance = attackDistance;
        agent.speed = patrolSpeed;
    }
    //봉인하기
    // 적 AI의 초기 스펙을 결정하는 셋업 메서드
    //public void Setup(float health, float damage,
    //    float runSpeed, float patrolSpeed, Color skinColor)
    //{
    //    // 체력 설정
    //    this.startingHealth = health;
    //    this.health = health;

    //    // 내비메쉬 에이전트의 이동 속도 설정
    //    this.runSpeed = runSpeed;
    //    this.patrolSpeed = patrolSpeed;

    //    this.damage = damage;

    //    // 렌더러가 사용중인 머테리얼의 컬러를 변경, 외형 색이 변함
    //    skinRenderer.material.color = skinColor;
    //}

    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (IsDead) return;//죽으면 반복문을 멈춤
        //상태값이 추적이고 추적상대가 존재한다면 참이됨
        if (state == State.Tracking && targetEntity != null)
        {
            //적과 플레이어간 거리를 구함
            var distance = Vector3.Distance(targetEntity.transform.position, transform.position);
            //플레이어와 적과의 거리가 공격거리보다 작다면
            if (distance <= attackDistance)
            {
                //공격시작
                BeginAttack();
            }
        }

        enemyHealth = Health;//체력을 유니티 에디터에서 보기위해 임시로 사용


        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        animator.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (IsDead) return;//죽으면 실행을 막음

        //공격하는 동안 자연스럽게 플레이어를 보게한다.
        if (state == State.AttackBegin || state == State.Attacking)
        {
            var lookRotation =
                Quaternion.LookRotation(targetEntity.transform.position - transform.position, Vector3.up);
            var targetAngleY = lookRotation.eulerAngles.y;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY,
                                        ref turnSmoothVelocity, turnSmoothTime);
        }
        //공격 상태일 경우
        if (state == State.Attacking)
        {
            var direction = transform.forward;//적의 앞방향값을 가짐
            var deltaDistance = agent.velocity.magnitude * Time.deltaTime;

            var size = Physics.SphereCastNonAlloc(attackRoot.position, attackRadius, direction, hits, deltaDistance,
                whatIsTarget);

            for (var i = 0; i < size; i++)
            {
                var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();

                if (attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
                {
                    var message = new DamageMessage();
                    message.amount = damage;
                    message.damagerLivingEntity = gameObject.GetComponent<LivingEntity>();
                    message.hitPoint = attackRoot.TransformPoint(hits[i].point);
                    message.hitNormal = attackRoot.TransformDirection(hits[i].normal);

                    attackTargetEntity.IsApplyDamage(message);

                    lastAttackedTargets.Add(attackTargetEntity);
                    break;
                }
            }
        }
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
        while (!IsDead)
        {
            if (isHasTarget)
            {
                if (state == State.Patrol)
                {
                    state = State.Tracking;
                    agent.speed = runSpeed;
                }

                // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                agent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                if (targetEntity != null) targetEntity = null;

                if (state != State.Patrol)
                {
                    state = State.Patrol;
                    agent.speed = patrolSpeed;
                }

                if (agent.remainingDistance <= 1f)
                {
                    var patrolPosition = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    agent.SetDestination(patrolPosition);
                }

                // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);

                // 모든 콜라이더들을 순회하면서, 살아있는 LivingEntity 찾기
                foreach (var collider in colliders)
                {
                    if (!IsTargetOnSight(collider.transform)) break;

                    var livingEntity = collider.GetComponent<LivingEntity>();

                    // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                    if (livingEntity != null && !livingEntity.IsDead)
                    {
                        // 추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;

                        // for문 루프 즉시 정지
                        break;
                    }
                }
            }

            // 0.2 초 주기로 처리 반복
            yield return new WaitForSeconds(0.2f);
        }
    }

    // 데미지를 입었을때 실행할 처리 체력부분
    public override bool IsApplyDamage(DamageMessage damageMessage)
    {
        if (!base.IsApplyDamage(damageMessage)) return false;

        if (targetEntity == null)
        {
            targetEntity = damageMessage.damagerLivingEntity.GetComponent<LivingEntity>();
        }
        EffectToolManager.Instance.GetEffect((int)ObjectList.flashHit, damageMessage.hitPoint, damageMessage.hitNormal);
        audioPlayer.PlayOneShot(hitClip);

        return true;
    }
    //공격시작
    public void BeginAttack()
    {
        state = State.AttackBegin;

        agent.isStopped = true;
        animator.SetTrigger("Attack");
    }
    //공격 가능
    public void EnableAttack()
    {
        state = State.Attacking;

        lastAttackedTargets.Clear();
    }
    //공격 중지
    public void DisableAttack()
    {
        state = State.Tracking;

        agent.isStopped = false;
    }

    private bool IsTargetOnSight(Transform target)
    {
        RaycastHit hit;

        var direction = target.position - eyeTransform.position;

        direction.y = eyeTransform.forward.y;

        if (Vector3.Angle(direction, eyeTransform.forward) > fieldOfView * 0.5f)
        {
            return false;
        }

        if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, whatIsTarget))
        {
            if (hit.transform == target) return true;
        }

        return false;
    }

    // 사망 처리
    public override void Die(int die)
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die(die);

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        GetComponent<Collider>().enabled = false;

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        agent.enabled = false;

        // 사망 애니메이션 재생
        animator.applyRootMotion = true;
        animator.SetTrigger("Die");

        // 사망 효과음 재생
        if (deathClip != null) audioPlayer.PlayOneShot(deathClip);
    }
}