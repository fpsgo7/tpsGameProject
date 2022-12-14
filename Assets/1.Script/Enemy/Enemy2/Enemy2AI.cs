using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy2AI : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Tracking,
        Attacking
    }

    public State state;

    public NavMeshAgent agent; // 경로계산 AI 에이전트
    public Animator animator; // 애니메이터 컴포넌트
    public Transform eyeTransform;
    public AudioSource audioPlayer; // 오디오 소스 컴포넌트

    public float runSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;//회전 지연속도
    private float turnSmoothVelocity;//좀비회전에 실시간 변화량
    private float attackDistance;
    public float fieldOfView = 50f;
    public float viewDistance = 10f;
    public float patrolSpeed = 3f;
    private WaitForSeconds wfs = new WaitForSeconds(0.2f);

    public readonly int hashSpeed = Animator.StringToHash("Speed");

    public LivingEntity targetEntity; // 추적할 대상
    public LayerMask whatIsTarget; // 추적 대상 레이어
    private Enemy2Health enemy2Health;
    private Enemy2Shooter enemy2Shooter;
    // 람다식을 활용한다.
    //targetEntity 가 널이아니고 추적할 대상이 죽지 않았다면  true 가 된다.
    private bool isTarget => targetEntity != null && !targetEntity.IsDead;


#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Quaternion leftRayRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * eyeTransform.transform.forward;
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
        enemy2Health = GetComponent<Enemy2Health>();
        enemy2Shooter = GetComponent<Enemy2Shooter>();
        //skinRenderer = GetComponentInChildren<Renderer>();//렌더러 연결



        attackDistance = 6;
        //네비게이션 에이전트의 값 초기화
        //agent.stoppingDistance = attackDistance;
        agent.speed = patrolSpeed;
    }
    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (enemy2Health.IsDead) return;//죽으면 반복문을 멈춤
        //상태값이 추적이고 추적상대가 존재한다면 참이됨
        if (state == State.Tracking && targetEntity != null)
        {
            //적과 플레이어간 거리를 구함
            float distance = Vector3.Distance(targetEntity.transform.position, transform.position);
            //플레이어와 적과의 거리가 공격거리보다 작다면
            if (distance <= attackDistance && IsTargetOnSight(targetEntity.transform))
            {
                Debug.Log("시야에 들어옴");
                agent.speed = 0;
                enemy2Shooter.isFire = true;
            }
            else
            {
                agent.speed = runSpeed;
                enemy2Shooter.isFire = false;
            }
                
        }
        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        animator.SetFloat(hashSpeed , agent.desiredVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (enemy2Health.IsDead) return;//죽으면 실행을 막음
        if(state == State.Tracking)
        {
            Quaternion lookRotation 
                = Quaternion.LookRotation
                (targetEntity.transform.position - transform.position, Vector3.up);
            float targetAngleY = lookRotation.eulerAngles.y;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle
                (transform.eulerAngles.y, targetAngleY,
                ref turnSmoothVelocity, turnSmoothTime);
        }
       
        
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
        while (!enemy2Health.IsDead)
        {
            if (isTarget)
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
                //랜덤 포인트 지정하여 순찰을 돔
                if (agent.remainingDistance <= 1f)
                {
                    Vector3 patrolPosition = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    agent.SetDestination(patrolPosition);
                }

                // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);

                // 모든 콜라이더들을 순회하면서, 살아있는 LivingEntity 찾기
                foreach (Collider collider in colliders)
                {
                    if (!IsTargetOnSight(collider.transform)) break;

                    LivingEntity livingEntity = collider.GetComponent<LivingEntity>();

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
            yield return wfs;
        }
    }
   
    //플레이어가 시야 내에 있는지 확인한다.
    private bool IsTargetOnSight(Transform target)
    {
        RaycastHit hit;

        Vector3 direction = target.position - eyeTransform.position;

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
}