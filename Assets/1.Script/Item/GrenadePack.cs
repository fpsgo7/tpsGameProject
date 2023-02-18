using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 수류탄 아이템으로 수류탄을 채워준다.
/// </summary>
public class GrenadePack : MonoBehaviour,IItem
{
    private FireGrenade fireGrenade;
    private WaitForSeconds wfs = new WaitForSeconds(0f);
    private void Awake()
    {
        fireGrenade = GameObject.FindWithTag("Player").GetComponent<FireGrenade>();
    }

    private void OnTriggerEnter(Collider other)
    {
        SoundToolManager.Instance.PlayOneShotSound
                   ((int)SoundList.pickUp, transform.position, 0.6f);
        if (other.CompareTag("Player"))
        {
            if (fireGrenade != null)
            {
                fireGrenade.GetGrenade();
            }
            StartCoroutine(ObjectToolManager.Instance.ReturnObjectByOrder
                            ((int)ObjectList.grenadePack, this.gameObject, wfs));
        }
        //else if (other.CompareTag("otherPlayer"))
        //{
        //    //다른 플레이어와 충돌 했을경우 
        //    StartCoroutine(GrenadePackPooling.ReturnObject(this.gameObject));
        //}
    }
}
