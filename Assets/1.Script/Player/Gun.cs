using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// PlayerShooter에 의하여 실제로 발사가 작동된다.
/// </summary>
public class Gun : MonoBehaviour
{
    //총의 상태를 담당함
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }
    public State state { get; private set; }//파라미터로 사용하여 내부에서만 상태를 바꿀수 있다.
    public enum Guns
    {
        SHOTGUN,
        RIFLEGUN,
        DMRGUN
    }
    public Guns guns { get; private set; }//파라미터로 사용하여 내부에서만 상태를 바꿀수 있다.
    //컴포넌트 형변수
    private PlayerShooter playerShooter;
    private LineRenderer bulletLineRenderer;
    //파티클 관련
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;
    //위치 관련
    public Transform fireTransform;
    public Transform leftHandMount;
    public Transform leftHandZoomIn;
    public Transform leftHandZoomOut;
    public Transform RightHandMount;
    //설정값
    public float damage = 10;
    public float fireDistance = 100f;

    public int ammoRemain = 100;
    public int magAmmo;
    public int magCapacity = 30;

    public float timeBetFire = 0.12f;//총알 발사 사이 간격
    public float reloadTime = 1.8f;//장전 시간
    [Range(0f, 10f)] public float maxSpread = 3f;//탄착군 최대 범위
    [Range(1f, 10f)] public float stability = 1f;//반동 증가 속도 해당 값을 증가하면 증가할 수 록 탄퍼짐 정도 감소
    [Range(0.01f, 3f)] public float restoreFromRecoilSpeed = 2f;//반동 감소 속도
    private float currentSpread;//현제 탄퍼짐값
    private float currentSpreadVelocity;//탄퍼짐값 변화량

    private float lastFireTime;//가장 최근 에 발싸가 된 시점

    private LayerMask excludeTarget;//총을 맞으면 안되는 대상

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;//총구의 위치 와 총알 이 닿는 위치 총 2곳
        bulletLineRenderer.enabled = false;//실수로 총알 렌더럴을 false 안해뒀을 경우 스크립트에서도 실행시켜준다.
    }

    //총의 초기화와 총을 교체할때 사용
    public void Setup(PlayerShooter gunHolder, float damage)
    {
        //총을 교체하면 그곳에맞는 gunHolder을 다시 맞춰줘야해서 총을 교체할때도 사용한다.
        this.playerShooter = gunHolder;
        excludeTarget = gunHolder.excludeTarget;//쏘지않을 대상의 값을 가져와 레이어에 저장
        if (damage != 0.0f)
            this.damage = damage;
    }
    //총이 활성화 되면 총을 초기화
    private void OnEnable()
    {
        magAmmo = magCapacity;
        currentSpread = 0f;
        lastFireTime = 0f;
        state = State.Ready;
        if(this.CompareTag("SHOTGUN"))
        {
            guns = Guns.SHOTGUN;
        }
        if (this.CompareTag("RIFLEGUN"))
        {
            guns = Guns.RIFLEGUN;
        }
        if (this.CompareTag("DMRGUN")) 
        {
            guns = Guns.DMRGUN;
        }
    }
    //비활 성화 되면 gun의 코루틴 비활성화
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    //외부에서 발사를 실행함
    public bool Fire(Vector3 aimTarget)
    {
        //Time.time >= lastFireTime + timeBetFire 현제시각이 발사 간격보다 큰경우
        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            Vector3 fireDirection1 = aimTarget - fireTransform.position;
            Vector3 fireDirection2 = aimTarget - fireTransform.position;
            Vector3 fireDirection3 = aimTarget - fireTransform.position;
            Vector3 fireDirection4 = aimTarget - fireTransform.position;
            Vector3 fireDirection5 = aimTarget - fireTransform.position;
            //탄퍼지기 정도 만들기
            float xError1 = Utility.GedRandomNormalDistribution(0f, currentSpread);//currentSpread 값이 클수록 xError의 값이 커질 확률이 높다.
            float yError1 = Utility.GedRandomNormalDistribution(0f, currentSpread);

            fireDirection1 = Quaternion.AngleAxis(yError1, Vector3.up)*fireDirection1;//y 축을 기준으로 y Error만큼 회전시킨다.
            fireDirection1 = Quaternion.AngleAxis(xError1, Vector3.right) * fireDirection1;//x 축일경우

            currentSpread += 1f / stability;//탄퍼짐 정도를 갈수록 늘려준다.
            lastFireTime = Time.time;
            //샷건 발사 추가
            if (this.gameObject.CompareTag("SHOTGUN"))
            {

                //탄퍼지기 정도 만들기
                float xError2 = Utility.GedRandomNormalDistribution(0.5f, currentSpread);//currentSpread 값이 클수록 xError의 값이 커질 확률이 높다.
                float yError2 = Utility.GedRandomNormalDistribution(0.5f, currentSpread);

                fireDirection2 = Quaternion.AngleAxis(yError2, Vector3.up) * fireDirection2;//y 축을 기준으로 y Error만큼 회전시킨다.
                fireDirection2 = Quaternion.AngleAxis(xError2, Vector3.right) * fireDirection2;//x 축일경우
                                                                                               //탄퍼지기 정도 만들기
                float xError3 = Utility.GedRandomNormalDistribution(0.5f, currentSpread);//currentSpread 값이 클수록 xError의 값이 커질 확률이 높다.
                float yError3 = Utility.GedRandomNormalDistribution(0.5f, currentSpread);

                fireDirection3 = Quaternion.AngleAxis(yError3, Vector3.up) * fireDirection3;//y 축을 기준으로 y Error만큼 회전시킨다.
                fireDirection3 = Quaternion.AngleAxis(xError3, Vector3.right) * fireDirection3;//x 축일경우
                                                                                               //탄퍼지기 정도 만들기
                float xError4 = Utility.GedRandomNormalDistribution(0.5f, currentSpread);//currentSpread 값이 클수록 xError의 값이 커질 확률이 높다.
                float yError4 = Utility.GedRandomNormalDistribution(0.5f, currentSpread);

                fireDirection4 = Quaternion.AngleAxis(yError4, Vector3.up) * fireDirection4;//y 축을 기준으로 y Error만큼 회전시킨다.
                fireDirection4 = Quaternion.AngleAxis(xError4, Vector3.right) * fireDirection4;//x 축일경우
                                                                                               //탄퍼지기 정도 만들기
                float xError5 = Utility.GedRandomNormalDistribution(0.5f, currentSpread);//currentSpread 값이 클수록 xError의 값이 커질 확률이 높다.
                float yError5 = Utility.GedRandomNormalDistribution(0.5f, currentSpread);

                fireDirection5 = Quaternion.AngleAxis(yError5, Vector3.up) * fireDirection5;//y 축을 기준으로 y Error만큼 회전시킨다.
                fireDirection5 = Quaternion.AngleAxis(xError5, Vector3.right) * fireDirection5;//x 축일경우
                Shot(fireTransform.position, fireDirection2);
                Shot(fireTransform.position, fireDirection3);
                Shot(fireTransform.position, fireDirection4);
                Shot(fireTransform.position, fireDirection5);
            }
            Shot(fireTransform.position, fireDirection1);

            magAmmo--;
            if (magAmmo <= 0)
                state = State.Empty;
            return true;
        }

        return false;
    }
    //실제 발사처리
    private void Shot(Vector3 startPoint, Vector3 direction)
    {
        RaycastHit hit;// 충돌대상 
        Vector3 hitPosition;//충돌 위치

        if(Physics.Raycast(startPoint,direction, out hit,fireDistance, ~excludeTarget))//~을 사용하여 조건문에서 ~을 가진 조건의 반대부분을 조건으로 사용한다.
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();// 충돌대상이 데미지를 받을수 있는 타입인지 검사

            if(target != null)
            {
                DamageMessage damageMessage;

                damageMessage.damagerLivingEntity = playerShooter.playerHealth.livingEntity;//공격을 가한측
                damageMessage.amount = damage;//데미지값
                damageMessage.hitPoint = hit.point;//피격 위치
                damageMessage.hitNormal = hit.normal;//피격위치 반대방향
                damageMessage.isHeadShot = false;
                target.ApplyDamage(damageMessage);
            }
            else
            {
                EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, hit.transform);
            }
            hitPosition = hit.point;

        }
        else
        {
            hitPosition = startPoint + direction * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));

    }
    //총구 섬광과 탄알 궤적 사용
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        //사격음
        SoundToolManager.Instance.PlayOneShotSound((int)SoundList.gunshot, transform.position, 1f);

        bulletLineRenderer.enabled = true;
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.03f);//대기시간

        bulletLineRenderer.enabled = false;
    }
    //외부에서 장전 함수를 호출 할수 있게함
    public bool Reload()
    {
        if(state == State.Reloading || ammoRemain <=0 || magAmmo >= magCapacity)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }
    // 실제 장전 처리
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        SoundToolManager.Instance.PlayOneShotSound((int)SoundList.gunReload, transform.position, 1f);

        yield return new WaitForSeconds(reloadTime);

        int ammoToFill = Mathf.Clamp(magCapacity - magAmmo,0,ammoRemain);
        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;

    }

    //중인 줌 아웃
    public void ZoomInFollow()
    {
        leftHandMount = leftHandZoomIn;
    }
    public void ZoomOutFollow()
    {
        leftHandMount = leftHandZoomOut;
    }

    private void Update()//업데이트 문 사용 필요**
    {
        currentSpread = Mathf.Clamp(currentSpread, 0f, maxSpread);//최대 탄퍼짐 이상으로 퍼지는 것을 막음
        currentSpread = Mathf.SmoothDamp(currentSpread, 0f, ref currentSpreadVelocity, 1f / restoreFromRecoilSpeed);//총을안쏘는 동안 반동을 감소 시킴
    }
}