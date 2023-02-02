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

    private Transform effectRoot = null;
    void Start()
    {
        if (effectRoot == null)
        {
            effectRoot = new GameObject("EffectRoot").transform;
            effectRoot.SetParent(transform);
        }
    }
    //원하는 이펙트를 원하는 대상에 생성한다.
    public GameObject EffectOneShot(int index, Vector3 position)
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
