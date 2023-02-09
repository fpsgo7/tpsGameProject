using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 오브젝트 메니져로 해당 스크립트를 가진 오브젝트 밑에 
/// 오브젝트 들이 달라 붙게된다.
/// </summary>
public class EffectToolManager : MonoBehaviour
{
    private static EffectToolManager instance;
    public static EffectToolManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<EffectToolManager>();

            return instance;
        }
    }

    private Transform effectRoot = null;// 이펙트 리스트담는 팩 오브젝트의 중심위치
    private Transform[] effectPacks = null;// 이펙트 들을 담는 팩
    private List<Queue<GameObject>> queueList= new List<Queue<GameObject>>();// 큐 들을 담고 있는 리스트
    private string[] effectDataNames;// 게임상 존재하는 데이타 이름들
    private ObjectClip[] effectClips;// 게임상 존제하는 이펙트 클립들
    private static WaitForSeconds[] wfs;// 삭제 시간 배열
    private const string wordS = "s";

    void Start()
    {
        if (effectRoot == null)
        {
            effectRoot = new GameObject("EffectRoot").transform;
            effectRoot.SetParent(transform);//해당 스크립트를 가진 오브젝트의 자식으로 생성된다.
        }
        // 모든 이펙트 클립과 이름들을 미리 가져온다.
        Queue<GameObject> poolingQueue;
        effectDataNames = new string[DataToolManager.EffectData().dataNames.Length];
        effectClips = new ObjectClip[DataToolManager.EffectData().objectClips.Length];
        wfs = new WaitForSeconds[DataToolManager.EffectData().dataNames.Length];
        for (int i = 0; i < DataToolManager.EffectData().objectClips.Length; i++)
        {
            effectDataNames[i] = DataToolManager.EffectData().dataNames[i];
            effectClips[i] = DataToolManager.EffectData().GetClip(i);
            wfs[i] = new WaitForSeconds(effectClips[i].objectTime);
        }
        // 생성할 오브젝트들을 보관하기 위한 오브젝트를 effectRoot 아래로 만든다.
        effectPacks = new Transform[effectDataNames.Length];
        for(int i = 0; i< effectDataNames.Length; i++)
        {
            poolingQueue = new Queue<GameObject>();// 오브젝트 큐
            effectPacks[i] = new GameObject(effectDataNames[i]+ wordS).transform;
            effectPacks[i].SetParent(effectRoot.transform);
            for(int j =0; j < effectClips[i].objectnecessary; j++)
            {
                poolingQueue.Enqueue(CreateEffect(effectClips[i], effectPacks[i]));
            }
            queueList.Add(poolingQueue);
        }
    }
    // 오브젝트 생성
    private GameObject CreateEffect(ObjectClip effectClip , Transform transform)
    {
        GameObject effectInstance = effectClip.Instantiate(transform.position);
        effectInstance.transform.SetParent(transform);
        effectInstance.SetActive(false);
        return effectInstance;
    }
    //오브젝트를 풀링하기
    public GameObject GetEffect(int index, Vector3 pos, Vector3 normal)
    {
        if (queueList[index].Count > 0)
        {
            GameObject effect = queueList[index].Dequeue();
            //Debug.Log(particle.name);
            effect.transform.position = pos;
            effect.transform.rotation = Quaternion.LookRotation(normal);
            effect.SetActive(true);// 활성화하여 보여줌
            StartCoroutine(ReturnObject(index, effect));
            return effect;
        }
        else
        {
            GameObject newEffect = CreateEffect(effectClips[index],transform);
            newEffect.transform.position = pos;
            newEffect.transform.rotation = Quaternion.LookRotation(normal);
            newEffect.SetActive(true);// 활성화하여 보여줌
            StartCoroutine(ReturnObject(index, newEffect));
            return newEffect;
        }
    }
    public IEnumerator ReturnObject(int index, GameObject effect)
    {
        yield return wfs[index];
        if (effect.activeSelf == true)
        {
            effect.gameObject.SetActive(false);
            effect.transform.SetParent(effectPacks[index]);
            queueList[index].Enqueue(effect);//다시 큐에 넣음
        }
    }
    public IEnumerator ReturnObjectByOrder(int index, GameObject effect, WaitForSeconds orderWfs)
    {
        yield return orderWfs;
        Debug.Log("돌아갑니다.");
        effect.gameObject.SetActive(false);
        effect.transform.SetParent(effectPacks[index]);
        queueList[index].Enqueue(effect);//다시 큐에 넣음

    }
}
