using System;
using System.Collections;
using UnityEngine;

public class EnemyTurretGun : MonoBehaviour
{
    //총의 상태를 담당함
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }
    public State state { get; private set; }//파라미터로 사용하여 내부에서만 상태를 바꿀수 있다.
    //컴포넌트 형변수
    public EnemyMiniTankGun gunHolder;
    public EnemyMiniTankHealth enemyMiniTankHealth;
    private LineRenderer bulletLineRenderer;
    //사운드 관련
    private AudioSource gunAudioPlayer;
    public AudioClip shotClip;
    public AudioClip reloadClip;
    //파티클 관련
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;
    //위치 관련
    public Transform fireTransform;
    // 기다리기 시간
    public WaitForSeconds wfs = new WaitForSeconds(0.03f);
    //설정값
    public float damage = 2;
    public float fireDistance = 100f;

    public int ammoRemain = 10000;
    public int magAmmo;
    public int magCapacity = 100;

    public float timeBetFire = 0.12f;//총알 발사 사이 간격
    public float reloadTime = 1.8f;//장전 시간

    [Range(0f, 10f)] public float maxSpread = 3f;//탄착군 최대 범위
    [Range(1f, 10f)] public float stability = 1f;//반동 증가 속도 해당 값을 증가하면 증가할 수 록 탄퍼짐 정도 감소
    [Range(0.01f, 3f)] public float restoreFromRecoilSpeed = 2f;//
    private float currentSpread;//현제 탄퍼짐값
    private float currentSpreadVelocity;//탄퍼짐값 변화량

    private float lastFireTime;//가장 최근 에 발싸가 된 시점

    public LayerMask excludeTarget;//총을 맞으면 안되는 대상

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;//총구의 위치 와 총알 이 닿는 위치 총 2곳
        bulletLineRenderer.enabled = false;//실수로 총알 렌더럴을 false 안해뒀을 경우 스크립트에서도 실행시켜준다.

    }
    //총이 활성화 되면 총을 초기화
    private void OnEnable()
    {
        magAmmo = magCapacity;
        currentSpread = 5f;
        lastFireTime = 0f;
        state = State.Ready;
    }
    //비활 성화 되면 총의 코루틴 비활성화
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    //외부에서 발사를 실행함
    public bool Fire()
    {
        //Time.time >= lastFireTime + timeBetFire 현제시각이 발사 간격보다 큰경우
        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot(fireTransform.position);//실제 발사 시작

            return true;
        }
        if(state == State.Empty)
        {
            Reload();
        }
        return false;
    }
    //실제 발사처리
    private void Shot(Vector3 startPoint)
    {
        RaycastHit hit;// 충돌대상 
        Vector3 hitPosition;//충돌 위치

        if (Physics.Raycast(startPoint, fireTransform.forward, out hit, fireDistance, ~excludeTarget))//~을 사용하여 조건문에서 ~을 가진 조건의 반대부분을 조건으로 사용한다.
        {
            var target = hit.collider.GetComponent<IDamageable>();// 충돌대상이 데미지를 받을수 있는 타입인지 검사

            if (target != null)
            {
                DamageMessage damageMessage;

                damageMessage.damagerLivingEntity = enemyMiniTankHealth.livingEntity;
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
            hitPosition = startPoint + fireTransform.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if (magAmmo <= 0)
            state = State.Empty;
    }
    //총구 섬광과 탄알 궤적 사용
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        gunAudioPlayer.PlayOneShot(shotClip);//소리는 중첩을 가능하게하기위해Play OneShot 사용

        bulletLineRenderer.enabled = true;
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);

        yield return wfs;//대기시간

        bulletLineRenderer.enabled = false;
    }
    //외부에서 장전 함수를 호출 할수 있게함
    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
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
        //gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        var ammoToFill = Mathf.Clamp(magCapacity - magAmmo, 0, ammoRemain);
        magAmmo += ammoToFill;
        //ammoRemain -= ammoToFill;

        state = State.Ready;

    }
    private void Update()
    {
        currentSpread = Mathf.Clamp(currentSpread, 0f, maxSpread);//최대 탄퍼짐 이상으로 퍼지는 것을 막음
        currentSpread = Mathf.SmoothDamp(currentSpread, 0f, ref currentSpreadVelocity, 1f / restoreFromRecoilSpeed);//총을안쏘는 동안 반동을 감소 시킴
    }
}