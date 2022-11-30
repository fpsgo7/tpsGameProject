using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackPooling : MonoBehaviour
{
    public static AmmoPackPooling Instance;

    [SerializeField]
    private GameObject poolingObject;
    private Queue<GameObject> poolingQueue = new Queue<GameObject>();
    private static WaitForSeconds wfs = new WaitForSeconds(0.1f);
    void Awake()
    {
        Instance = this;
        Initalize(5);
    }
    // 오브젝트 생성
    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(poolingObject, transform);
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
    public static GameObject GetObjet(Vector3 point)
    {
        if (Instance.poolingQueue.Count > 0)
        {
            var obj = Instance.poolingQueue.Dequeue();//큐에서 하나 꺼내옴
            obj.transform.position = point;
            obj.gameObject.SetActive(true);// 활성화하여 보여줌
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.transform.position = point;
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }
    //다시 오브젝트를 반납하기
    public static IEnumerator ReturnObject(GameObject item)
    {
        yield return wfs;
        item.gameObject.SetActive(false);
        item.transform.SetParent(Instance.transform);
        Instance.poolingQueue.Enqueue(item);//다시 큐에 넣음
    }
}
