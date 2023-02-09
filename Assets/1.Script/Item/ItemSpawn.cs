using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 적들이 죽으면 소비형 아이템을 랜덤으로 생성해준다.
/// </summary>
public class ItemSpawn : MonoBehaviour
{
    private Vector3 point = new Vector3(0,0,0);
    public void Spawn()
    {
        point = new Vector3(transform.position.x, 0.5f , transform.position.z);
        // 생성할 아이템을 무작위로 하나 선택
        int itemToCreate = Random.Range(0, 3);
        //아이템을 해당 스크립트 를 가진 대상이 죽으면 생성하게함
        if(itemToCreate == 0)
            EffectToolManager.Instance.GetEffect((int)ObjectList.ammoPack, point, point);
        if (itemToCreate == 1)
            EffectToolManager.Instance.GetEffect((int)ObjectList.grenadePack, point, point);
        if (itemToCreate == 2)
            EffectToolManager.Instance.GetEffect((int)ObjectList.healthPack, point, point);
    }
}