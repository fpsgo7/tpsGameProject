using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 어디에 데미지를 받았는지 구분하여
/// 해드샷 구현에 쓰인다.
/// </summary>
public class DamageablePosition : MonoBehaviour, IDamageable
{
    public LivingEntity livingEntity;
    public enum HitPosition
    {
        HEAD,
        BODDY
    }
    public HitPosition hitPosition;
    // 몸에 맞을경우 일반데미지가 뜨지만
    // 머리에 맞을경우 추가 데미지를 적용한다.
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
