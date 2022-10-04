using UnityEngine;
using UnityEngine.AI;

public class ItemSpawn : MonoBehaviour
{
    public GameObject[] items;

    public void Spawn()
    {
        // 생성할 아이템을 무작위로 하나 선택
        GameObject itemToCreate = items[Random.Range(0, items.Length)];
        //아이템을 해당 스크립트 를 가진 대상이 죽으면 생성하게함
        GameObject item = Instantiate(itemToCreate , transform.position , Quaternion.identity);

    }
}