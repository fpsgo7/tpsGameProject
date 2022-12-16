using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePack : MonoBehaviour,IItem
{
    private FireGrenade fireGrenade;

    private void Awake()
    {
        fireGrenade = GameObject.FindWithTag("Player").GetComponent<FireGrenade>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fireGrenade != null)
            {
                fireGrenade.GetGrenade();
            }

            StartCoroutine(GrenadePackPooling.ReturnObject(this.gameObject));
        }
        //else if (other.CompareTag("otherPlayer"))
        //{
        //    //다른 플레이어와 충돌 했을경우 
        //    StartCoroutine(GrenadePackPooling.ReturnObject(this.gameObject));
        //}
    }
}
