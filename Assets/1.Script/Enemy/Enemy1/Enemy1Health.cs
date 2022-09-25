using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Health : LivingEntity
{
    private Enemy1AI enemy1AI;

    public AudioClip hitClip; // 피격시 재생할 소리
    public AudioClip deathClip; // 사망시 재생할 소리

    public float enemyHealth;// 체력 확인용

    private void Start()
    {
        enemy1AI = GetComponent<Enemy1AI>();
    }
    private void FixedUpdate()
    {
        enemyHealth = health;//체력을 유니티 에디터에서 보기위해 임시로 사용

    }

    //봉인하기
    // 적 AI의 초기 스펙을 결정하는 셋업 메서드
    //public void Setup(float health, float damage,
    //    float runSpeed, float patrolSpeed, Color skinColor)
    //{
    //    // 체력 설정
    //    this.startingHealth = health;
    //    this.health = health;

    //    // 내비메쉬 에이전트의 이동 속도 설정
    //    this.runSpeed = runSpeed;
    //    this.patrolSpeed = patrolSpeed;

    //    this.damage = damage;

    //    // 렌더러가 사용중인 머테리얼의 컬러를 변경, 외형 색이 변함
    //    skinRenderer.material.color = skinColor;
    //}

    // 데미지를 입었을때 실행할 처리 체력부분
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        if (enemy1AI.targetEntity == null)
        {
            enemy1AI.targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
        }

        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        enemy1AI.audioPlayer.PlayOneShot(hitClip);

        return true;
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        GetComponent<Collider>().enabled = false;

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        enemy1AI.agent.enabled = false;

        // 사망 애니메이션 재생
        enemy1AI.animator.applyRootMotion = true;
        enemy1AI.animator.SetTrigger("Die");

        // 사망 효과음 재생
        if (deathClip != null) enemy1AI.audioPlayer.PlayOneShot(deathClip);
    }
}
