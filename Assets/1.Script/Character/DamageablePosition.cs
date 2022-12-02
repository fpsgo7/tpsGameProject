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
        if(hitPosition == HitPosition.HEAD)
        {
            damageMessage.amount *= 1.5f;
            damageMessage.isHeadShot = true;
            livingEntity.ApplyDamage(damageMessage);
        }
        if(hitPosition == HitPosition.BODDY)
        {
            livingEntity.ApplyDamage(damageMessage);
        }
        return true;
    }
}
