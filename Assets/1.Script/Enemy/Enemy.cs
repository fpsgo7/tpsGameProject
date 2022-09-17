using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//전처리기로 해당 파트를 사용하는 것은
//에디터에서만 실행시켜 테스트 용도로만 활용한다.
#if UNITY_EDITOR
using UnityEditor;//네임 스페이스 선언
#endif

public class Enemy : LivingEntity
{
    //상태값
    public enum State
    {
        Patrol, //순찰
        Tracking, //추적
        AttackBegin, //공격 시작
        Attacking //공격
    }

    public State state;

    private NavMeshAgent agent;//네비게이션 에이전트
    private Animator animator;//에니메이터

    public Transform attackRoot;//공격 반지름
    public Transform eyeTransform;//좀비가 적을 감지하기위한 것
    //오디오 관련
    private AudioSource audioPlayer;
    public AudioClip hitClip;
    public AudioClip deathClip;
    //피부색 결정
    private Renderer skinRenderer;

    public float runSpeed = 10f;//속도
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;//회전 지연속도
    private float turnSmoothVelocity;//좀비회전에 실시간 변화량
    //공격관련
    public float damage = 30f;
    public float attackRadius = 2f; //공격 반경
    private float attackDistance;//공격 거리

    public float fieldOfView = 50f;//시야각
    public float viewDistance = 10f;//보는 거리
    public float patrolSpeed = 1f;//순찰 속도

    [HideInInspector] public LivingEntity targetEntity;//적이 추적할 대상
    public LayerMask whatIsTarget;//추적대상을 정할 레이어


    private RaycastHit[] hits = new RaycastHit[10];//적의 공격을 범위 기반이라서 여러 개의 충돌포인트가 생긴다.
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();//공격 직전 대상을 담아두는 값 공격 중복을 막는다.
    // 람다식을 활용한다.
    //targetEntity 가 널이아니고 추적할 대상이 죽지 않았다면  true 가 된다.
    private bool hasTarget => targetEntity != null && !targetEntity.dead;


#if UNITY_EDITOR//전처리기로 유니티에서만 실행

    private void OnDrawGizmosSelected()//좀비의 시야와 공격범위를 그려준다.
    {
        if (attackRoot != null)
        {
            //공격 범위 그림
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(attackRoot.position, attackRadius);
        }

        if (eyeTransform != null)
        {
            //적의 시야를 표현
            var leftEyeRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
            var leftRayDirection = leftEyeRotation * transform.forward;
            Handles.color = new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
        }



    }

#endif

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        skinRenderer = GetComponentInChildren<Renderer>();//자식 오브젝트에서 가져온다.
        //공격 거리 구하기
        var attackPibot = attackRoot.position;//공격용 오브젝트 위치를 넣는다.
        attackPibot.y = transform.position.y;//이후 공격용 오브젝트 위치를 적 객체의 높이 와 맞춰준다.
        attackDistance = Vector3.Distance(transform.position, attackPibot) + attackRadius;//적객체의 몸과 공격 오브젝트 거리 에다가, 범위를 더하여 공격 거리를 구한다.

        agent.stoppingDistance = attackDistance;//적이 공격거리에 도착하면 멈추기 위하여 네비게이션 에 값을 건든다.
        agent.speed = patrolSpeed;// 정찰 속도로 시작
    }
    //적이 생성될때 적의 체력 속도 등 스팩을 결정한다.
    //적 생성기에서 값을 결정하여 생성한다.
    public void Setup(float health, float damage,
        float runSpeed, float patrolSpeed, Color skinColor)
    {
        //매계변수로 온 값들을 적용한다.
        this.startingHealth = health;
        this.health = health;

        this.damage = damage;
        this.runSpeed = runSpeed;
        this.patrolSpeed = patrolSpeed;

        skinRenderer.material.color = skinColor;

        agent.speed = patrolSpeed;//awake 함수에서 실행했지만 한번더 처음 스피드를 고정한다.
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (dead)
        {
            return;
        }

        if (state == State.Tracking)
        {
            var distance = Vector3.Distance(targetEntity.transform.position, transform.position);
            //플레이어와 적과의 거리가 공격거리보다 작다면
            if (distance <= attackDistance)
            {
                //공격시작
                BeginAttack();
            }
        }

        animator.SetFloat("Speed", agent.desiredVelocity.magnitude);//네비게이션에 입력된 속도를 보내준다.
    }
    //상대방의 콜라이더를 감지한후 상대방에게 대미지를 주는 기능
    private void FixedUpdate()
    {
        if (dead)
            return;
        //공격하는 대상을 바라보도록 해주는 코드
        if (state == State.AttackBegin || state == State.AttackBegin)
        {
            var lookRotation = Quaternion.LookRotation(targetEntity.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;// y 축 각도를 활용한다.
            //SmoothDampAngle로 하여 부드럽게 회전하게한다.
            targetAngleY
                = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY,
                ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;
        }

        if (state == State.Attacking)
        {
            //공격 궤적을 만듬
            var direction = transform.forward;
            var deltaDistance = agent.velocity.magnitude * Time.deltaTime;
            //SphereCast로 구하면 새로운 배열을 생성해서  사양이 높아져
            //배열을 재활용하기위해 SphereCastNonAlloc 사용한다.
            //적이 공격을 휘두를때 닿는 콜라이더를 가져온다.
            var size = Physics.SphereCastNonAlloc
                (attackRoot.position, attackRadius, direction, hits, deltaDistance, whatIsTarget);
            for (var i = 0; i < size; i++)
            {
                //가져온대상이 LivingEntity가 있는 경우
                var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();

                if (attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
                {
                    var message = new DamageMessage();
                    message.amount = damage;
                    message.damager = gameObject;

                    if (hits[i].distance <= 0f)
                    {
                        message.hitPoint = attackRoot.position;
                    }
                    else
                    {
                        message.hitPoint = hits[i].point;
                    }

                    message.hitPoint = hits[i].point;

                    attackTargetEntity.ApplyDamage(message);
                    lastAttackedTargets.Add(attackTargetEntity);
                    break;
                }
            }
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!dead)//사망하지 않는동안
        {
            if (hasTarget)//추적대상이 존재하는 경우
            {
                if (state == State.Patrol)//아직 추적상태가 아닌경우
                {
                    state = State.Tracking;//상태를 추적상태로 바꾼다.
                    agent.speed = runSpeed;//속도 증가
                }
                //네브메쉬 에이전트 에 목적지를 업데이트함
                agent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                //추적대상이 죽은 경우 다른 대상을 추적하기위하여 사용
                if (targetEntity != null)
                    targetEntity = null;

                if (state != State.Patrol)
                {
                    state = State.Patrol;
                    agent.speed = patrolSpeed;
                }
                //remainingDistance 도착까지 남은 거리
                if (agent.remainingDistance <= 1f)
                {
                    //인공지능이 정찰할 랜덤한 위치
                    var patrolTargetPosition
                        = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    agent.SetDestination(patrolTargetPosition);
                }

                //플레이어 감지 시스템
                //먼저 탐지거리만큼 눈위치를 기준으로 콜라이더를 찾아 가져온다.
                var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);
                foreach (var collider in colliders)
                {
                    //상대방이 시야내에 없으면 무시
                    if (!IsTargetOnSight(collider.transform))
                    {
                        continue;
                    }

                    var livingEntity = collider.GetComponent<LivingEntity>();

                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        //공격 대미지 작용이 실패했으면 false 로 바로 리턴하여 밑의 
        //문장이 실행되지 않게한다.
        if (!base.ApplyDamage(damageMessage)) return false;
        //공격대상이 없는 상태에서 공격을 받을경우
        //공격을 가한대상을 바로 추적하게한다.
        if (targetEntity == null)
        {
            targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
        }
        //피격효과 생성
        EffectManager.Instance.PlayHitEffect
            (damageMessage.hitPoint, damageMessage.hitNormal, transform,
            EffectManager.EffectType.Flesh);
        audioPlayer.PlayOneShot(hitClip);

        return true;
    }

    public void BeginAttack()
    {
        state = State.AttackBegin;

        agent.isStopped = true;//추적 중단
        animator.SetTrigger("Attack");//에니메이터 공격 활성화
    }
    //EnableAttack ,과 DisableAttack 은 에니메이션 이벤트에서 실행된다.
    //실제로 데미지가 들어가는 지점
    public void EnableAttack()
    {
        Debug.Log(" 공격 허가");
        state = State.Attacking;

        lastAttackedTargets.Clear();
    }
    //공격이 끝나는 지점
    public void DisableAttack()
    {
        Debug.Log(" 공격 미허가");
        if (hasTarget)
        {
            state = State.Tracking;
        }
        else
        {
            state = State.Patrol;
        }

        agent.isStopped = false;
    }
    //시야 범위내에있는지 확인한다.
    private bool IsTargetOnSight(Transform target)
    {
        //눈에 위치에서 타겍의 위치를 향하는 방향 백터 생성
        var direction = target.position - eyeTransform.position;//방향 구하기
        direction.y = eyeTransform.forward.y;//시야와 같은 높이로 한다. 수평으로 적을 추적하기 때문
        //시야각 바깥에 있는 경우
        if (Vector3.Angle(direction, eyeTransform.forward) > fieldOfView * 0.5f)
        {
            return false;
        }

        direction = target.position - eyeTransform.position;

        RaycastHit hit;
        if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, whatIsTarget))
        {
            if (hit.transform == target)
            {
                return true;
            }
        }
        return false;
    }

    public override void Die()
    {
        base.Die();

        GetComponent<Collider>().enabled = false;

        agent.enabled = false;//네비게이션 에이전트를 종료함

        animator.applyRootMotion = true;//스크립트가아닌 에니메이션 컨트롤러 내에서 통제하게한다.
        animator.SetTrigger("Die");

        audioPlayer.PlayOneShot(deathClip);
    }
}