﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObjectPooling : MonoBehaviour
{
    public static ExplosionObjectPooling Instance;

    [SerializeField]
    private GameObject poolingGrenadeExplosionObject;
    private Queue<GameObject> poolingQueue = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        Initalize(5);
    }
    // 오브젝트 생성
    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(poolingGrenadeExplosionObject,transform);
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
    public static GameObject GetObjet(Transform transform)
    {
        if (Instance.poolingQueue.Count > 0)
        {
            Debug.Log("생성하기");
            var obj = Instance.poolingQueue.Dequeue();//큐에서 하나 꺼내옴
            obj.transform.position = transform.position;
            obj.gameObject.SetActive(true);// 활성화하여 보여줌
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.transform.position = transform.position;
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }
    //다시 오브젝트를 반납하기
    public static IEnumerator ReturnObject(GameObject explosion)
    {
        Debug.Log("되돌리기");
        yield return new WaitForSeconds(2f);
        explosion.gameObject.SetActive(false);
        explosion.transform.SetParent(Instance.transform);
        Instance.poolingQueue.Enqueue(explosion);//다시 큐에 넣음
    }
}