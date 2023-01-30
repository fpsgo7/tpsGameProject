using UnityEngine;
/// <summary>
/// 체력 충전 아이템으로 체력 충전 키트를 더해준다.
/// </summary>
public class HealthPack : MonoBehaviour,IItem
{
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.GetHealthKit();
            }

            StartCoroutine(HealthPackPooling.ReturnObject(this.gameObject));
        }
        //else if (other.CompareTag("otherPlayer"))
        //{
        //    //다른 플레이어와 충돌 했을경우 
        //    StartCoroutine(HealthPackPooling.ReturnObject(this.gameObject));
        //}
    }
}