using UnityEngine;
/// <summary>
/// 체력 충전 아이템으로 체력 충전 키트를 더해준다.
/// </summary>
public class HealthPack : MonoBehaviour,IItem
{
    private PlayerHealth playerHealth;
    private WaitForSeconds wfs = new WaitForSeconds(0f);

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
            StartCoroutine(EffectToolManager.Instance.ReturnObjectByOrder
                           ((int)ObjectList.healthPack, this.gameObject, wfs));
        }
        //else if (other.CompareTag("otherPlayer"))
        //{
        //    //다른 플레이어와 충돌 했을경우 
        //    StartCoroutine(HealthPackPooling.ReturnObject(this.gameObject));
        //}
    }
}