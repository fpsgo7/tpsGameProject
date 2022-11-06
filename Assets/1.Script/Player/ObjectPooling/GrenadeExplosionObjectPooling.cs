using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosionObjectPooling : MonoBehaviour
{
    public static GrenadeExplosionObjectPooling Instance;

    [SerializeField]
    private GameObject poolingGrenadeExplosionObject;
    private Queue<Explosion> poolingQueue = new Queue<Explosion>();

    void Awake()
    {
        Instance = this;
        Initalize(5);
    }
    // 오브젝트 생성
    private Explosion CreateNewObject()
    {
        var newObj = Instantiate(poolingGrenadeExplosionObject, transform).GetComponent<Explosion>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }
    // 실제 오브젝트 생성 실행을휘한 함수
    private void Initalize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            poolingQueue.Enqueue(CreateNewObject());
        }
    }
    //오브젝트를 풀링하기
    public static Explosion GetObjet(Transform transform)
    {
        if (Instance.poolingQueue.Count > 0)
        {
            var obj = Instance.poolingQueue.Dequeue();//큐에서 하나 꺼내옴
            obj.transform.SetParent(null);//
            obj.transform.position = transform.position;
            obj.gameObject.SetActive(true);// 활성화하여 보여줌
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.transform.position = transform.position;
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }
    //다시 오브젝트를 반납하기
    public static void ReturnObject(Explosion explosion)
    {
        explosion.gameObject.SetActive(false);
        explosion.transform.SetParent(Instance.transform);
        Instance.poolingQueue.Enqueue(explosion);//다시 큐에 넣음
    }
}
