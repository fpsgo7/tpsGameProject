using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePack : MonoBehaviour, IItem
{
    public void Use(GameObject target)
    {
        Debug.Log("충돌이 일어남");
        var fireGrenade = target.GetComponent<FireGrenade>();

        if (fireGrenade != null)
        {
            fireGrenade.GetGrenade();
        }

        Destroy(gameObject);
    }
}
