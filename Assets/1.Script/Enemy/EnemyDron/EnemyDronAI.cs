using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDronAI : MonoBehaviour
{

    public AudioSource audioPlayer; // 오디오 소스 컴포넌트

    private float runSpeed = 3f;
    private int damage = 30;
    private float attackDistance;
    private float viewDistance = 10f;

    public LivingEntity targetEntity; // 추적할 대상
    public LayerMask whatIsTarget; // 추적 대상 레이어
    private EnemyDronHealth enemyHealth;
    public Vector3 t_dir;//미사일 추격 각도 변수
    private RaycastHit[] rayHits; //충돌대상
    public GameObject explosion;//폭발
    // 람다식을 활용한다.
    //targetEntity 가 널이아니고 추적할 대상이 죽지 않았다면  true 가 된다.
    private bool hasTarget => targetEntity != null && !targetEntity.dead;

    private void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();//오디오 연결
        enemyHealth = GetComponent<EnemyDronHealth>();
        //skinRenderer = GetComponentInChildren<Renderer>();//렌더러 연결

        //네비게이션 에이전트의 값 초기화
        attackDistance = 4;
    }
    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (enemyHealth.dead) return;//죽으면 반복문을 멈춤
        //상태값이 추적이고 추적상대가 존재한다면 참이됨
        if (targetEntity != null)
        {
            //적과 플레이어간 거리를 구함
            var distance = Vector3.Distance(targetEntity.transform.position, transform.position);
            //플레이어와 적과의 거리가 공격거리보다 작다면
            if (distance <= attackDistance && enemyHealth.dead == false)
            {
                //폭발 명령
                enemyHealth.Die(0);
            }
        }

        if (hasTarget && enemyHealth.dead == false)
        {
            transform.position += transform.forward * runSpeed * Time.deltaTime;//표적이 있다면 미사일을 위로 가속

         
            if (transform.position.y < 3)
            {
                t_dir = (targetEntity.transform.position - transform.position + new Vector3(0, 3, 0)).normalized;
            }
            else
            {
                t_dir = (targetEntity.transform.position - transform.position).normalized;
            }
            //드론의 각도 설정
            transform.forward = Vector3.Lerp(transform.forward, t_dir, 0.25f);
        }
    }
    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
        while (!enemyHealth.dead)
        {
            if (!hasTarget)
            {
                if (targetEntity != null) targetEntity = null;

                // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                var colliders = Physics.OverlapSphere(transform.position, viewDistance, whatIsTarget);

                // 모든 콜라이더들을 순회하면서, 살아있는 LivingEntity 찾기
                foreach (var collider in colliders)
                {
                    var livingEntity = collider.GetComponent<LivingEntity>();

                    // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                    if (livingEntity != null && !livingEntity.dead)
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
    //폭파 작동
    public void Explosion()
    {

        //폭발범위에 들어간 적들을 찾아냄
        rayHits = Physics.SphereCastAll(transform.position,
            5,
            Vector3.up, 0f,
            LayerMask.GetMask("Player"));
        ExplosionAttack();
        GameObject obj = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(obj, 0.5f);
        Destroy(this.gameObject, 0.1f);
    }

    public void ExplosionAttack()
    {

        //폭파 범위안의 있는 적들을 반복문으로 접근함
        foreach (RaycastHit hitobj in rayHits)
        {
            if (hitobj.transform.GetComponent<LivingEntity>())
            {
                Debug.Log("공격 성공");
                hitobj.transform.GetComponent<LivingEntity>().ApplyDamage(damage);
            }
               
        }
    }
}
