using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public void Use(GameObject target)
    {
        var playerHealth = target.GetComponent<PlayerHealth>();

        if(playerHealth != null)
        {
            playerHealth.GetHealthKit();
        }

        Destroy(gameObject);
    }
}