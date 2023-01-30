using UnityEngine;
/// <summary>
/// 총알 아이템으로 총알을 체워준다.
/// </summary>
public class AmmoPack : MonoBehaviour,IItem
{
    private PlayerShooter playerShooter;
    public int ammo = 30;

    private void Awake()
    {
        playerShooter = GameObject.FindWithTag("Player").GetComponent<PlayerShooter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerShooter != null && playerShooter.gun != null)
            {
                playerShooter.gun.ammoRemain += ammo;
            }

            StartCoroutine(AmmoPackPooling.ReturnObject(this.gameObject));
        }
        //else if (other.CompareTag("otherPlayer"))
        //{
        //    //다른 플레이어와 충돌 했을경우 
        //    StartCoroutine(AmmoPackPooling.ReturnObject(this.gameObject));
        //}
    }
}