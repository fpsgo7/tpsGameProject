using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiniTankHealth : LivingEntity
{
    private EnemyMiniTankAI enemyAI;

    public AudioClip hitClip; // 피격시 재생할 소리
    public AudioClip deathClip; // 사망시 재생할 소리

    //private Renderer skinRenderer; // 렌더러 컴포넌트

    public float enemyHealth;// 체력 확인용

    private void Start()
    {
        enemyAI = GetComponent<EnemyMiniTankAI>();
        MaxHealth = 200;
    }
    //private void FixedUpdate()
    //{
    //    enemyHealth = health;//체력을 유니티 에디터에서 보기위해 임시로 사용

    //}

    // 데미지를 입었을때 실행할 처리 체력부분
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage))
        {
            Debug.Log("데미지 실패");
            return false;
        }
        if (enemyAI.targetEntity == null)
        {
            //enemyAI.targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
            enemyAI.TargetUpdate(damageMessage.damager.GetComponent<LivingEntity>());
        }
        //이펙트 추가?

        //사운드
        //enemyAI.audioPlayer.PlayOneShot(hitClip);

        return true;
    }
    //폭파 데미지를 입을 경우 실행할 처리 부분
    public override bool ApplyDamage(int damage, GameObject damager)
    {
        if (!base.ApplyDamage(damage, damager)) return false;

        if (enemyAI.targetEntity == null)
        {
            enemyAI.targetEntity = damager.GetComponent<LivingEntity>();
        }
        enemyAI.audioPlayer.PlayOneShot(hitClip);

        return true;
    }
    // 사망 처리
    public override void Die(int die)
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die(die);

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        GetComponent<Collider>().enabled = false;

        enemyAI.Explosion();
    }
}
