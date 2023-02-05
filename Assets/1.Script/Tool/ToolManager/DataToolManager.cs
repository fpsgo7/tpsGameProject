using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 모든 데이터의 시작이된다.
/// </summary>
public class DataToolManager
{
    // 다른 스크립트에서도 해당 스크립트에 접근을 위한 변수 생성
    private static SoundData soundData = null;
    private static EffectData effectData = null;
    //이펙트 데이터를 가져오기위해 사용
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
