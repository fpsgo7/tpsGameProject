using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 수류탄이 폭파되며 수류탄 대미지가 적용된다.
/// </summary>
public class Grenade : MonoBehaviour
{
    public float speed = 500.0f;//수류탄 속도
    public GameObject explosion;
    private LivingEntity FireManEntity;
    private int damage = 50;
    private RaycastHit[] rayHits;
    public void SetGrenadeInfo(LivingEntity FireManEntity, Transform firePos)
    {
        this.FireManEntity = FireManEntity;
        transform.position = firePos.position;
    }
    public void Shoot()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        Invoke("ExplosionGrenade", 1.0f);
    }
    private void ExplosionGrenade()
    {
        
        //폭발범위에 들어간 적들을 찾아냄
        rayHits = Physics.SphereCastAll(transform.position,
            10,
            Vector3.up, 0f,
            LayerMask.GetMask("Enemy"));
        ExplosionAttack();
        EffectManager.Instance.ExplosionEffect(this.transform);
        GrenadeObjectPooling.ReturnObject(this);
    }
    private void ExplosionAttack()
    {
       
        //폭파 범위안의 있는 적들을 반복문으로 접근함
        foreach(RaycastHit hitobj in rayHits)
        {
            if (hitobj.transform.GetComponent<LivingEntity>())
                hitobj.transform.GetComponent<LivingEntity>().IsApplyDamage(damage, FireManEntity);
        }
    }
}
