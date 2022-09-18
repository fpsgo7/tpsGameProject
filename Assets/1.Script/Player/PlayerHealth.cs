using System.Collections;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private PlayerInput playerInput;//플레이어 입력
    private Animator animator;//에니메이터
    private AudioSource playerAudioPlayer;//오디오 소스
    //오디오 클립
    public AudioClip deathClip;
    public AudioClip hitClip;
    //변수 들
    public int restoreHealth;
    public int restoreHealthMax;

    private bool restoreHealthStart = true;

    //컴포넌트 연결
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAudioPlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        restoreHealthMax = 500;
        UIManager.Instance.RestoreHealthMax(restoreHealthMax);

        restoreHealth = 0;
    }
    private void Update()
    {
        if (playerInput.restoreHealth && health < 100)
        { 
            StartCoroutine(RestoreHealthSlider());
        }
        else if(restoreHealth >=1)
        {
            restoreHealth = 0;
            UIManager.Instance.UpdateRestoreHealth(restoreHealth);
            UIManager.Instance.UpdateRestoreHealthEnd();
            restoreHealthStart = true;
        }
        if(restoreHealth >= 1 && restoreHealthStart == true)
        {
            UIManager.Instance.UpdateRestoreHealthStart();
            restoreHealthStart = false;
        }
        if (restoreHealth >= restoreHealthMax)
        {
            StopCoroutine(RestoreHealthSlider());
            UIManager.Instance.UpdateRestoreHealthEnd();
            RestoreHealth();
            restoreHealth = 0;
            restoreHealthStart = true;
        }
    }

    //base로 상속받은 내용 사용후 내용을 추가하여 사용
    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateUI();
    }

    //헬스슬라이더 체우기
    private IEnumerator RestoreHealthSlider()
    {
        restoreHealth += 1;
        UIManager.Instance.UpdateRestoreHealth(restoreHealth);
        yield return true;
    }

    public override void RestoreHealth()
    {
        base.RestoreHealth();
        UpdateUI();
    }

    private void UpdateUI()
    {
        //UIManager에 체력 업데이트를 체력값을 보내어 사용하며
        //죽음 상태인 경우 0값을 보낸다.
        UIManager.Instance.UpdateHealthText(dead ? 0f : health);
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        //데미지 적용이 실패한경우 return false를 시킨다.
        if (!base.ApplyDamage(damageMessage))
            return false;
        //피격효과 (피격지점, 피격각도, 플레이어 위치, 사용할 이펙트)
        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        //피격사운드 재생
        playerAudioPlayer.PlayOneShot(hitClip);

        UpdateUI();

        return true;//공격이 성공한 것을 알림
    }

    public override void Die()
    {
        base.Die();

        playerAudioPlayer.PlayOneShot(deathClip);
        animator.SetTrigger("Die");

        UpdateUI();
    }
}