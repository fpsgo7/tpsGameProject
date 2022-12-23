using System.Collections;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private PlayerInput playerInput;//플레이어 입력
    private Animator animator;//에니메이터
    private AudioSource playerAudioPlayer;//오디오 소스
     //변수 들
    private int restoreHealth;//체력회복 게이지
    private int restoreHealthMax;//체력회복 게이지 최댁값
    private int healthKit;// 체력회복 킷
    public bool isRestoringHealth = false;
    //오디오 클립
    public AudioClip deathClip;
    public AudioClip hitClip;
    //bool 형
    [HideInInspector] public bool isRestoreHealthProceeding = false;
    [HideInInspector] public bool isInvincibility = false;

    //컴포넌트 연결
    protected override void Awake()
    {
        livingEntity = GetComponent<LivingEntity>();
        playerInput = GetComponent<PlayerInput>();
        playerAudioPlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        restoreHealthMax = 100;
        UIPlayerInfo.Instance.SetHealthSliderMaxValue(restoreHealthMax);//체력 회복 
    }
    private void FixedUpdate()
    {
        //회복 문장
        if (isRestoringHealth && Health < maxHealth && healthKit >=1)
        {
            RestoreHealthSlider();
        }
        else if(restoreHealth >=1)
        {
            RestoreHealthStop();
        }
    }

    //base로 상속받은 내용 사용후 내용을 추가하여 사용
    protected override void OnEnable()
    {
        restoreHealth = 0;
        healthKit = 5;
        base.OnEnable();
        UpdateUI();
    }

    //헬스슬라이더 체우기
    public void RestoreHealthSlider()
    {
        restoreHealth += 1;
        UIPlayerInfo.Instance.RestoreHealthSlideValue(restoreHealth);
        // 체력 회복 UI 활성화
        if (restoreHealth >= 1 && isRestoreHealthProceeding == false)
        {
            UIPlayerInfo.Instance.ActiveRestoreHealthSlider();
            isRestoreHealthProceeding = true;
        }
        // 체력 회복 완료
        if (restoreHealth >= restoreHealthMax)
        {
            UIPlayerInfo.Instance.InactiveRestoreHealtSlider();
            RestoreHealth();
            restoreHealth = 0;
            isRestoreHealthProceeding = false;
        }
    }
    //체력 회복 중단
    private void RestoreHealthStop()
    {
        restoreHealth = 0;
        UIPlayerInfo.Instance.RestoreHealthSlideValue(restoreHealth);
        UIPlayerInfo.Instance.InactiveRestoreHealtSlider();
        isRestoreHealthProceeding = false;
    }


    public override void RestoreHealth()
    {
        base.RestoreHealth();
        healthKit -= 1;
        UpdateUI();
    }

    public void GetHealthKit()
    {
        healthKit += 1;
        UpdateUI();
    }

    private void UpdateUI()
    {
        //UIManager에 체력 업데이트를 체력값을 보내어 사용하며
        //죽음 상태인 경우 0값을 보낸다.
        UIPlayerInfo.Instance.SetHealthText(IsDead ? 0f : Health);
        UIPlayerInfo.Instance.SetHealthKitText(healthKit);
    }

    public override bool IsApplyDamage(DamageMessage damageMessage)
    {
        if (isInvincibility == true)// 데미지 적용을 무적상태 동안은 막음
            return false;
        //데미지 적용이 실패한경우 return false를 시킨다.
        if (!base.IsApplyDamage(damageMessage))
            return false;
        //피격효과 (피격지점, 피격각도, 플레이어 위치, 사용할 이펙트)
        // EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        //피격사운드 재생
        playerAudioPlayer.PlayOneShot(hitClip);

        UpdateUI();

        return true;//공격이 성공한 것을 알림
    }
    //폭파 대미지용
    public override bool IsApplyDamage(int damage, LivingEntity damager)
    {
        if (isInvincibility == true)// 데미지 적용을 무적상태 동안은 막음
            return false;
        //데미지 적용이 실패한경우 return false를 시킨다.
        if (!base.IsApplyDamage(damage, damager))
            return false;
        //피격사운드 재생
        //playerAudioPlayer.PlayOneShot(hitClip);

        UpdateUI();

        return true;//공격이 성공한 것을 알림
    }

    public override void Die(int die)
    {
        base.Die(die);

        //playerAudioPlayer.PlayOneShot(deathClip);
        animator.SetTrigger("Die");

        UpdateUI();
    }

    public void EqipmentWear(float shield)
    {
        if (Health >= maxHealth)
        {
            maxHealth = startingPlayerHealth;//체력 초기화
            maxHealth += shield;//방어구만큼 최대체력 증가.
            Health = maxHealth;
        }
        else
        {
            maxHealth = startingPlayerHealth;//체력 초기화
            maxHealth += shield;//방어구만큼 최대체력 증가.
        }
        UIPlayerInfo.Instance.SetHealthMaxSlider(maxHealth);//방어구 수정으로 슬라이더 최대값 수정 추가
        UpdateUI();
    }
}