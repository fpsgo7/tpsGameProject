using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Health : LivingEntity
{
    private Enemy2AI enemy2AI;

    public AudioClip hitClip; // 피격시 재생할 소리
    public AudioClip deathClip; // 사망시 재생할 소리

    public float enemyHealth;// 체력 확인용

    public readonly int hashDie = Animator.StringToHash("Die");
    public readonly int hashDieJump = Animator.StringToHash("DieJump");

    private void Start()
    {
        enemy2AI = GetComponent<Enemy2AI>();
        MaxHealth = 100;
    }
    //private void FixedUpdate()
    //{
    //    enemyHealth = health;//체력을 유니티 에디터에서 보기위해 임시로 사용
        
    //}

    // 데미지를 입었을때 실행할 처리 체력부분
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        if (enemy2AI.targetEntity == null)
        {
            enemy2AI.targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
        }

        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        enemy2AI.audioPlayer.PlayOneShot(hitClip);

        return true;
    }

    //폭파 데미지를 입을 경우 실행할 처리 부분
    public override bool ApplyDamage(int damage, GameObject damager)
    {
        if (!base.ApplyDamage(damage, damager)) return false;

        if (enemy2AI.targetEntity == null)
        {
            enemy2AI.targetEntity = damager.GetComponent<LivingEntity>();
        }
        enemy2AI.audioPlayer.PlayOneShot(hitClip);

        return true;
    }
    // 사망 처리
    public override void Die(int die)
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die(die);

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        GetComponent<Collider>().enabled = false;

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        enemy2AI.agent.enabled = false;

        // 사망 애니메이션 재생
        enemy2AI.animator.applyRootMotion = true;
        if(die == 0)
            enemy2AI.animator.SetTrigger(hashDie);
        if(die == 1)
            enemy2AI.animator.SetTrigger(hashDieJump);

        // 사망 효과음 재생
        if (deathClip != null) enemy2AI.audioPlayer.PlayOneShot(deathClip);
    }
}
