using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePack : MonoBehaviour
{
    public FireGrenade fireGrenade; //= target.GetComponent<FireGrenade>();
    public void Use(GameObject target)
    {
        Debug.Log("충돌이 일어남");
        fireGrenade = target.GetComponent<FireGrenade>();

        if (fireGrenade != null)
        {
            fireGrenade.GetGrenade();
        }

        Destroy(gameObject);
    }
}
