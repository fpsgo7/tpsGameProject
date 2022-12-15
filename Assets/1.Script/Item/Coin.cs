using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    public int score = 200;

    public void Use()
    {
        GameManager.Instance.AddScore(score);
        Destroy(gameObject);
    }
}