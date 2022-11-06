using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeObjectPooling : MonoBehaviour
{
    public static GrenadeObjectPooling Instance;

    [SerializeField]
    private GameObject poolingGrenadeObject;
    private Queue<Grenade> poolingGrenadeQueue = new Queue<Grenade>();

    void Awake()
    {
        Instance = this;
        Initalize(5);
    }
    // 오브젝트 생성
    private Grenade CreateNewObject()
    {
        var newObj = Instantiate(poolingGrenadeObject, transform).GetComponent<Grenade>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }
    // 실제 오브젝트 생성 실행을휘한 함수
    private void Initalize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            poolingGrenadeQueue.Enqueue(CreateNewObject());
        }
    }
    //오브젝트를 풀링하기
    public static Grenade GetObjet()
    {
        if (Instance.poolingGrenadeQueue.Count > 0)
        {
            var obj = Instance.poolingGrenadeQueue.Dequeue();//큐에서 하나 꺼내옴
            obj.transform.SetParent(null);//
            obj.gameObject.SetActive(true);// 활성화하여 보여줌
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }
    //다시 오브젝트를 반납하기
    public static void ReturnObject(Grenade grenade)
    {
        grenade.gameObject.SetActive(false);
        grenade.transform.SetParent(Instance.transform);
        Instance.poolingGrenadeQueue.Enqueue(grenade);//다시 큐에 넣음
    }
}
