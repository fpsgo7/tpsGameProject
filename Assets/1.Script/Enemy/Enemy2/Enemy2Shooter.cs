using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// enemy2 의 사격 기능 관리
/// </summary>
public class Enemy2Shooter : MonoBehaviour
{
    private Enemy2Health enemy2Health;
    protected Transform target;
    public Transform firePos;// 총알 발사위치를 위한변수
    public Transform gunPivot;//총의 위치
    public Transform leftHandMount;// 왼쪽손
    public Transform RightHandMount;//오른쪽손
    private Animator enemyAnimator;
    private AudioSource enemyAudio;

    protected LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러
    public MeshRenderer muzzleFlash;
    public LayerMask excludeTarget;//총을 맞으면 안되는 대상
    protected WaitForSeconds wsReload;// 코루틴형 장전 시간
    private WaitForSeconds wsShot = new WaitForSeconds(0.03f);

    //hash 값으로 애니메이션을 조절한다.
    protected readonly int hashFire = Animator.StringToHash("Fire");
    protected readonly int hashReload = Animator.StringToHash("Reload");

    public bool isReload = false;//재장전상태
    public bool isFire = false;
    protected float nextFire;
    protected float fireRate;
    protected float damping;// 회전속도

    protected float reloadTime;//제장전시간
    public int maxBullet;//최대 탄수
    public int currBullet;//현제 탄수
    public bool isDeath = false;//죽음상탠
    protected float fireDistance = 50f; // 사정거리
    protected float damage;//데미지
    
    private void Awake()
    {
        enemy2Health = GetComponent<Enemy2Health>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();

        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
        //값 설정 상속형식으로 변수 생성없이 값 설정만으로 적들의 위력을 설정가능하다.
        reloadTime = 2.0f;
        maxBullet = 30;
        currBullet = 30;
        damage = 1;
        //damage = 0; 테스트용
        wsReload = new WaitForSeconds(reloadTime);
        nextFire = 0;
        fireRate = 0.1f;
        //총 회전을 위한 변수값
        damping = 10.0f;
    }

    //추적 대상을 각각 다른스크립트에서 찾으면 오류날수있어 
    //다른 추적대상에서 얻은 추적대상을 가져와 적용한다.
   // public void findPlayer(GameObject target)
   // {   //playerTr의 값을 구하여 적이 플레이어를 조준할수 있게함
     //   this.target = target.transform;
    //}

    public void Update()
    {
        //죽음상태가 true인경우 반복문을 실행하지 못하게함
        if (enemy2Health.IsDead == true)
        {
            enemyAnimator.SetBool(hashFire, false);
        }
        else
        {

            //실질적인발사
            if (!isReload && isFire == true)
            {
                //Time.time은 스크립트가 실행됐을때부터 흘러가는 시간이며 nextFire에는 Time.time+딜레이 시간이 들어간다.
                if (Time.time >= nextFire)
                {
                    Fire();
                    //Fire트리거를 활성화 하고 총소리를 플레이해준다.
                    enemyAnimator.SetBool(hashFire, true);
                    //랜덤한 딜레이을 위해 Random 함수를 사용하며 상단의 using System;을 지워 사용한다.
                    nextFire = Time.time + fireRate + Random.Range(0.0f, 0.1f);
                }
            }
            else
            {
                enemyAnimator.SetBool(hashFire, false);
            }

        }
    }

    public virtual void Fire()
    {
        // 실제 발사 처리는 호스트에게 대리

        Shot();
        //현재 총알에서 1을 뺀 수를 최대 총알수로 하며
        //최대 총알수로 나눈 나머지가 0이면 true 아니라면 false로 같게한다.
        isReload = (--currBullet % maxBullet == 0);//0에서 숫자를 나눠야 나머지가 0이된다.
        //해당 조건이 충족하면 코루틴을 실행하고 장전 애니메이션을 시작한다.
        if (isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    public virtual void Shot()
    {
        SoundToolManager.Instance.PlayOneShotSound((int)SoundList.gunshot, transform.position, 1f);

        // 레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 총알이 맞은 곳을 저장할 변수
        Vector3 hitPosition;//충돌 위치

        if (Physics.Raycast(firePos.position, firePos.forward, out hit, fireDistance, ~excludeTarget))//~을 사용하여 조건문에서 ~을 가진 조건의 반대부분을 조건으로 사용한다.
        {
            var target = hit.collider.GetComponent<IDamageable>();// 충돌대상이 데미지를 받을수 있는 타입인지 검사

            if (target != null)
            {
                DamageMessage damageMessage;

                damageMessage.damagerLivingEntity = enemy2Health.livingEntity;
                damageMessage.amount = damage;//데미지값
                damageMessage.hitPoint = hit.point;//피격 위치
                damageMessage.hitNormal = hit.normal;//피격위치 반대방향
                damageMessage.isHeadShot = false;
                target.ApplyDamage(damageMessage);
            }
            else
            {
                ObjectToolManager.Instance.GetObject((int)ObjectList.commonHit, hit.point, hit.normal);
            }
            hitPosition = hit.point;

        }
        else
        {
            hitPosition = firePos.position + firePos.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));//NullReference 오류

    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    protected virtual IEnumerator ShotEffect(Vector3 hitPosition)
    {

        StartCoroutine(ShowMuzzleFlash());//Fire 함수에서 코루틴 함수를 실행해줍니다.
        // 선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(0, firePos.position);//nullReference 오류해결
        bulletLineRenderer.SetPosition(1, hitPosition);
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return wsShot;

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }

    //코루틴으로 사용할 ShowMuzzleFlash 함수를 선언한다.
    //총알 발사섬광을 표현한다.
    public virtual IEnumerator ShowMuzzleFlash()
    {
        var wfs = new WaitForSeconds(Random.Range(0.05f, 0.2f));
        muzzleFlash.enabled = true;//화염이 나타나야 하므로 true로한다.
        //Euler함수를 이용해서 z축으로 랜덤하게 회전하게 한다.
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        //회전값을 muzzleFlash 의 localRotation 에 넣어준다.
        muzzleFlash.transform.localRotation = rot;
        //크기 스케일도 렌덤하게 지정해서 1~2 배 사이로 하여 localScale에 넣는다.
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.0f, 2.0f);
        //랜덤한 좌표값을 muzzleFlash의 material offset 에 넣어준다.
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;//0,0 부터 0.5,0.5까지 랜덤 좌표값 생성
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);//_MainTex는 미리 지정되어 있는 Property Name으로 Diffuse를 나타낸다.

        //0.05초 부터 0.2초 까지 랜덤하게 코루틴 함수를 호출한다. 즉 잠시동안 멈춰준다.
        yield return wfs;
        muzzleFlash.enabled = false;

    }
    //제장전 IEnumerator형 메소드
    public virtual IEnumerator Reloading()
    {
        muzzleFlash.enabled = false;//장전하는 동안 총구 화염이 안보이게 한다.
        enemyAnimator.SetTrigger(hashReload);//Reload 트리거를 활성화
        SoundToolManager.Instance.PlayOneShotSound((int)SoundList.gunReload, transform.position, 1f);
        //ReloadTime 만큼 대기한다.
        yield return wsReload;
        //총알을 채워준다.
        currBullet = maxBullet;
        isReload = false;//false로 변경하며 재장전 끝
    }

    //총을 쥐는 것을 다룸
    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = enemyAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        enemyAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        enemyAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        enemyAnimator.SetIKPosition(AvatarIKGoal.RightHand, RightHandMount.position);
        enemyAnimator.SetIKRotation(AvatarIKGoal.RightHand, RightHandMount.rotation);

        if (isReload == true)
            return;

        enemyAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        //enemyAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        enemyAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        //enemyAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);
    }
}
