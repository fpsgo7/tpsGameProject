using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 모든 데이터의 시작이된다.
/// </summary>
public class DataToolManager : MonoBehaviour
{
    // 다른 스크립트에서도 해당 스크립트에 접근을 위한 변수 생성
    private static SoundData soundData = null;
    private static EffectData effectData = null;
    void Start()//시작하자마자 이팩트 데이터를 가지고 있다.
    {
        //데이터가 없을경우 에 시작한다.
        if (effectData == null)
        {
            //EffectData를 스크립터블 오브젝트 형식으로 생성하며
            // 해당 대상을 변수에 넣어줘  
            // 변수에 넣은 대상에 로드한다.
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();
        }
        if (soundData == null)
        {
            // 스크립터블 오브젝트형태로 사운드 데이터 생성후 로드한다.
            soundData = ScriptableObject.CreateInstance<SoundData>();
            soundData.LoadData();
        }
    }
    // effectData 를 생성하여 로드한후 반환하는 함수 이다.
    public static EffectData EffectData()
    {
        if (effectData == null)
        {
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();
        }
        return effectData;
    }
    // 위처럼 데이터클래스 를 생성하여 로드한후 반환하는 함수이다.
    public static SoundData SoundData()
    {
        if (soundData == null)
        {
            soundData = ScriptableObject.CreateInstance<SoundData>();
            soundData.LoadData();
        }
        return soundData;
    }
}
