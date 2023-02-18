﻿using UnityEngine;
/// <summary>
/// 총알 아이템으로 총알을 체워준다.
/// </summary>
public class AmmoPack : MonoBehaviour,IItem
{
    private PlayerShooter playerShooter;
    private WaitForSeconds wfs = new WaitForSeconds(0f);
    public int ammo = 30;

    private void Awake()
    {
        playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundToolManager.Instance.PlayOneShotSound
                ((int)SoundList.pickUp, transform.position, 0.6f);
            if (playerShooter != null && playerShooter.gun != null)
            {
                playerShooter.gun.ammoRemain += ammo;
            }
            StartCoroutine(ObjectToolManager.Instance.ReturnObjectByOrder
                ((int)ObjectList.ammoPack, this.gameObject, wfs));
        }
    }
}