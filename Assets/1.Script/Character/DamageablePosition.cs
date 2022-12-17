using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageablePosition : MonoBehaviour, IDamageable
{
    public LivingEntity livingEntity;
    public enum HitPosition
    {
        HEAD,
        BODDY
    }
    public HitPosition hitPosition;
    public bool ApplyDamage(DamageMessage damageMessage)
    {
        damageMessage.amount *= Random.Range(0.8f, 1.2f);
        if(hitPosition == HitPosition.HEAD)
        {
            damageMessage.amount *= 1.5f;
            damageMessage.isHeadShot = true;
            livingEntity.IsApplyDamage(damageMessage);
        }
        if(hitPosition == HitPosition.BODDY)
        {
            livingEntity.IsApplyDamage(damageMessage);
        }
        return true;
    }
}
