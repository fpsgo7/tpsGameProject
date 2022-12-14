using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Health : LivingEntity
{
    private Enemy1AI enemy1AI;

    public AudioClip hitClip; // 피격시 재생할 소리
    public AudioClip deathClip; // 사망시 재생할 소리
    private Collider enemy1Collider;

    public float enemyHealth;// 체력 확인용
    public readonly int hashDie = Animator.StringToHash("Die");

    protected override void Awake()
    {
        base.Awake();
        enemy1AI = GetComponent<Enemy1AI>();
        //MaxHealth = 1000; 테스트용
        maxHealth=100;
        enemy1Collider = GetComponent<Collider>();
    }
    //private void FixedUpdate()
    //{
    //    enemyHealth = health;//체력을 유니티 에디터에서 보기위해 임시로 사용

    //}



    // 데미지를 입었을때 실행할 처리 체력부분
    public override bool IsApplyDamage(DamageMessage damageMessage)
    {
        if (!base.IsApplyDamage(damageMessage)) return false;

        if (enemy1AI.targetEntity == null)
        {
            enemy1AI.targetEntity = damageMessage.damagerLivingEntity;
        }

        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        enemy1AI.audioPlayer.PlayOneShot(hitClip);

        return true;
    }
    //폭파 데미지를 입을 경우 실행할 처리 부분
    public override bool IsApplyDamage(int damage ,LivingEntity damager)
    {
        if (!base.IsApplyDamage(damage,damager)) return false;

        if (enemy1AI.targetEntity == null)
        {
            enemy1AI.targetEntity = damager;
        }
        enemy1AI.audioPlayer.PlayOneShot(hitClip);

        return true;
    }

    // 사망 처리
    public override void Die(int die)
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die(die);

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        enemy1Collider.enabled = false;

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        enemy1AI.agent.enabled = false;

        // 사망 애니메이션 재생
        enemy1AI.animator.applyRootMotion = true;
        enemy1AI.animator.SetTrigger(hashDie);

        // 사망 효과음 재생
        if (deathClip != null) enemy1AI.audioPlayer.PlayOneShot(deathClip);
        Destroy(gameObject, 3f);
    }
}
