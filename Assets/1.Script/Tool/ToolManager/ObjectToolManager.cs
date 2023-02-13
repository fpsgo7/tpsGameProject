using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 오브젝트 메니져로 해당 스크립트를 가진 오브젝트 밑에 
/// 오브젝트 들이 달라 붙게된다.
/// </summary>
public class ObjectToolManager : MonoBehaviour
{
    private static ObjectToolManager instance;
    public static ObjectToolManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<ObjectToolManager>();

            return instance;
        }
    }

    private Transform objectRoot = null;// 이펙트 리스트담는 팩 오브젝트의 중심위치
    private Transform[] objectPacks = null;// 이펙트 들을 담는 팩
    private List<Queue<GameObject>> objectQueueList= new List<Queue<GameObject>>();// 큐 들을 담고 있는 리스트
    private string[] objectDataNames;// 게임상 존재하는 데이타 이름들
    private ObjectClip[] objectClips;// 게임상 존제하는 이펙트 클립들
    private static WaitForSeconds[] wfs;// 삭제 시간 배열
    private const string wordS = "s";

    void Start()
    {
        if (objectRoot == null)
        {
            objectRoot = new GameObject("EffectRoot").transform;
            objectRoot.SetParent(transform);//해당 스크립트를 가진 오브젝트의 자식으로 생성된다.
        }
        // 모든 이펙트 클립과 이름들을 미리 가져온다.
        Queue<GameObject> poolingQueue;
        objectDataNames = new string[DataToolManager.EffectData().dataNames.Length];
        objectClips = new ObjectClip[DataToolManager.EffectData().objectClips.Length];
        wfs = new WaitForSeconds[DataToolManager.EffectData().dataNames.Length];
        for (int i = 0; i < DataToolManager.EffectData().objectClips.Length; i++)
        {
            objectDataNames[i] = DataToolManager.EffectData().dataNames[i];
            objectClips[i] = DataToolManager.EffectData().GetClip(i);
            wfs[i] = new WaitForSeconds(objectClips[i].objectTime);
        }
        // 생성할 오브젝트들을 보관하기 위한 오브젝트를 effectRoot 아래로 만든다.
        objectPacks = new Transform[objectDataNames.Length];
        for(int i = 0; i< objectDataNames.Length; i++)
        {
            poolingQueue = new Queue<GameObject>();// 오브젝트 큐
            objectPacks[i] = new GameObject(objectDataNames[i]+ wordS).transform;
            objectPacks[i].SetParent(objectRoot.transform);
            for(int j =0; j < objectClips[i].objectnecessary; j++)
            {
                poolingQueue.Enqueue(CreateObject(objectClips[i], objectPacks[i]));
            }
            objectQueueList.Add(poolingQueue);
        }
    }
    // 오브젝트 생성
    private GameObject CreateObject(ObjectClip objectClip , Transform transform)
    {
        GameObject objectInstance = objectClip.Instantiate(transform.position);
        objectInstance.transform.SetParent(transform);
        objectInstance.SetActive(false);
        return objectInstance;
    }
    //오브젝트를 풀링하기
    public GameObject GetObject(int index, Vector3 pos, Vector3 normal)
    {
        if (objectQueueList[index].Count > 0)
        {
            GameObject objectPool = objectQueueList[index].Dequeue();
            //Debug.Log(particle.name);
            objectPool.transform.position = pos;
            objectPool.transform.rotation = Quaternion.LookRotation(normal);
            objectPool.SetActive(true);// 활성화하여 보여줌
            StartCoroutine(ReturnObject(index, objectPool));
            return objectPool;
        }
        else
        {
            GameObject newObject = CreateObject(objectClips[index],transform);
            newObject.transform.position = pos;
            newObject.transform.rotation = Quaternion.LookRotation(normal);
            newObject.SetActive(true);// 활성화하여 보여줌
            StartCoroutine(ReturnObject(index, newObject));
            return newObject;
        }
    }
    public IEnumerator ReturnObject(int index, GameObject returnObject)
    {
        yield return wfs[index];
        if (returnObject.activeSelf == true)
        {
            returnObject.gameObject.SetActive(false);
            returnObject.transform.SetParent(objectPacks[index]);
            objectQueueList[index].Enqueue(returnObject);//다시 큐에 넣음
        }
    }
    public IEnumerator ReturnObjectByOrder(int index, GameObject returnObject, WaitForSeconds orderWfs)
    {
        yield return orderWfs;
        Debug.Log("돌아갑니다.");
        returnObject.gameObject.SetActive(false);
        returnObject.transform.SetParent(objectPacks[index]);
        objectQueueList[index].Enqueue(returnObject);//다시 큐에 넣음

    }
}
