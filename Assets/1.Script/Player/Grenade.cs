using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float speed = 500.0f;//수류탄 속도
    public GameObject explosion;
    public GameObject FireMan;
    private int damage = 50;
    private RaycastHit[] rayHits;
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
        Invoke(nameof(DestroyGrenade), 1f);
        var Explosion = GrenadeExplosionObjectPooling.GetObjet(transform);
        StartCoroutine(GrenadeExplosionObjectPooling.ReturnObject(Explosion));
    }
    private void ExplosionAttack()
    {
       
        //폭파 범위안의 있는 적들을 반복문으로 접근함
        foreach(RaycastHit hitobj in rayHits)
        {
            if (hitobj.transform.GetComponent<LivingEntity>())
                hitobj.transform.GetComponent<LivingEntity>().ApplyDamage(damage,FireMan);
        }
    }
    //오브젝트 풀링방식으로 없에기
    private void DestroyGrenade()
    {
        GrenadeObjectPooling.ReturnObject(this);
    }
}
