using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 이펙트 메니져로 해당 스크립트를 가진 오브젝트 밑에 
/// 이펙트들이 달라 붙게된다.
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

    private Transform effectRoot = null;// 이펙트 경로
    private Transform[] effectPacks = null;// 이펙트 들을 담는 팩
    private GameObject[] poolingObjects;// 풀링할 대상
    private Queue<GameObject> poolingQueue = new Queue<GameObject>();// 오브젝트 큐
    private static WaitForSeconds wfs = new WaitForSeconds(2f);// 삭제 시간
    private const string wordS = "s";

    void Start()
    {
        if (effectRoot == null)
        {
            effectRoot = new GameObject("EffectRoot").transform;
            effectRoot.SetParent(transform);//해당 스크립트를 가진 오브젝트의 자식으로 생성된다.
        }
        // 모든 이펙트 클립과 이름들을 미리 가져온다.
        string[] effectDataNames = new string[DataToolManager.EffectData().dataNames.Length];
        EffectClip[] clip = new EffectClip[DataToolManager.EffectData().effectClips.Length];
        for (int i = 0; i < DataToolManager.EffectData().effectClips.Length; i++)
        {
            effectDataNames[i] = DataToolManager.EffectData().dataNames[i];
            clip[i] = DataToolManager.EffectData().GetClip(i);
            //Debug.Log(effectDataNames[i]+"의 클립"+clip[i].effectName);
        }
        // 생성할 오브젝트들을 보관하기 위한 오브젝트를 effectRoot 아래로 만든다.
        effectPacks = new Transform[effectDataNames.Length];
        for(int i = 0; i< effectDataNames.Length; i++)
        {
            effectPacks[i] = new GameObject(effectDataNames[i]+ wordS).transform;
            effectPacks[i].SetParent(effectRoot.transform);
        }
        // 오브젝트 풀링 을 활용하기 위해 오브젝트들을 미리생성한다.

    }
    //원하는 이펙트를 원하는 대상에 생성한다.
    public GameObject EffectUse(int index, Vector3 position)
    {
        // 스킓터형 변수를 생성후 해당 포지션에 생성하며
        // 오븢게트에 넣는다. 
        // 이후 대상을 액티브 트루로 하여 이펙트를 보여준다.
        EffectClip clip = DataToolManager.EffectData().GetClip(index);
        GameObject effectInstance = clip.Instantiate(position);
        effectInstance.SetActive(true);
        return effectInstance;
    }
}
