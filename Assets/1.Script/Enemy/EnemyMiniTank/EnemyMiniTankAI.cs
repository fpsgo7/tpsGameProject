using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 적 미니 탱크의 기본 기능을 담는다.
/// </summary>
public class EnemyMiniTankAI : MonoBehaviour
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

    public AudioSource audioPlayer; // 오디오 소스 컴포넌트

    private float runSpeed = 3f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;//회전 지연속도
    private float turnSmoothVelocity;//회전에 실시간 변화량

    public int damage = 30;//자폭 대미지
    private float attackDistance;

    public float fieldOfView = 50f;
    public float viewDistance = 10f;
    public float patrolSpeed = 3f;

    public LivingEntity targetEntity; // 추적할 대상
    public LayerMask whatIsTarget; // 추적 대상 레이어
    private EnemyMiniTankHealth enemyMiniTankHealth;
    private EnemyMiniTankGun enemyMiniTankGun;
    private WaitForSeconds wfs = new WaitForSeconds(0.2f);

    //적의 공격을 범위 기반이라서 여러 개의 충돌포인트가 생긴다.
    private RaycastHit[] rayHits; //충돌대상
    public GameObject explosion;//폭발
    // 람다식을 활용한다.
    //targetEntity 가 널이아니고 추적할 대상이 죽지 않았다면  true 가 된다.
    private bool isTarget => targetEntity != null && !targetEntity.IsDead;

    private void Awake()
    {
        //컴포넌트 연결
        agent = GetComponent<NavMeshAgent>();//네비게이션 연결
        audioPlayer = GetComponent<AudioSource>();//오디오 연결
        enemyMiniTankHealth = GetComponent<EnemyMiniTankHealth>();
        enemyMiniTankGun = GetComponent<EnemyMiniTankGun>();//스크립트 연결
        //skinRenderer = GetComponentInChildren<Renderer>();//렌더러 연결

        //네비게이션 에이전트의 값 초기화
        attackDistance = 5;
        agent.stoppingDistance = attackDistance;
        agent.speed = patrolSpeed;
    }
    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (enemyMiniTankHealth.IsDead) return;//죽으면 반복문을 멈춤
        //상태값이 추적이고 추적상대가 존재한다면 참이됨
        if (state == State.Tracking && targetEntity != null)
        {
            //적과 플레이어간 거리를 구함
            float distance = Vector3.Distance(targetEntity.transform.position, transform.position);
            //플레이어와 적과의 거리가 공격거리보다 작다면
            if (distance <= attackDistance)
            {
                agent.speed = 0;
            }
            else
            {
                agent.speed = runSpeed;
               
            }
        }
    }
    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
        while (!enemyMiniTankHealth.IsDead)
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

                if (agent.remainingDistance <= 1f)
                {
                    Vector3 patrolPosition = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    agent.SetDestination(patrolPosition);
                }

                // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(transform.position, viewDistance, whatIsTarget);

                // 모든 콜라이더들을 순회하면서, 살아있는 LivingEntity 찾기
                foreach (Collider collider in colliders)
                { 
                    LivingEntity livingEntity = collider.GetComponent<LivingEntity>();

                    // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                    if (livingEntity != null && !livingEntity.IsDead)
                    {
                        // 추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;
                        enemyMiniTankGun.SameTarget(targetEntity.transform);
                        // for문 루프 즉시 정지
                        break;
                    }
                }
            }

            // 0.2 초 주기로 처리 반복
            yield return wfs;
        }
    }

    //폭파 작동
    public void Explosion()
    {
        //폭발범위에 들어간 적들을 찾아냄
        rayHits = Physics.SphereCastAll(transform.position,
            5,
            Vector3.up, 0f,
            LayerMask.GetMask("Player"));
        ExplosionAttack();
        ObjectToolManager.Instance.GetObject((int)ObjectList.explosion, this.transform.position, this.transform.position);
        SoundToolManager.Instance.PlayOneShotSound((int)SoundList.explosion, transform.position, 0.7f);
        this.gameObject.SetActive(false);
    }

    public void ExplosionAttack()
    {

        //폭파 범위안의 있는 적들을 반복문으로 접근함
        foreach (RaycastHit hitobj in rayHits)
        {
            if (hitobj.transform.GetComponent<LivingEntity>())
            {
                Debug.Log("공격 성공");
                hitobj.transform.GetComponent<LivingEntity>().IsApplyDamage(damage,enemyMiniTankHealth.livingEntity);
            }

        }
    }

    public void TargetUpdate(LivingEntity livingEntity)
    {
        targetEntity = livingEntity;
        enemyMiniTankGun.SameTarget(targetEntity.transform);
    }
}
