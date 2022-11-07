using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // 싱글톤
    private static EffectManager m_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<EffectManager>();
            return m_Instance;
        }
    }
    //상태값 
    public enum EffectType
    {
        Common,//일반 파티클 효과
        Flesh// 피격 효과
    }
    
    public ParticleSystem commonHitEffectPrefab;//일반 파티클 변수
    public ParticleSystem fleshHitEffectPrefab;//피격 파티클 변수
    //(이팩트 위치, 방향, 이펙트 부모, 사용할 이펙트 타입)
    public void PlayHitEffect(Vector3 pos, Vector3 normal, Transform parent = null, EffectType effectType = EffectType.Common)
    {
        //var targetPrefab1 = commonHitEffectPrefab;
        ////만약 생명채가 피격당할 경우이면 사용하는 이펙트를 바꾼다.
        //if (effectType == EffectType.Flesh)
        //{
        //    targetPrefab1 = fleshHitEffectPrefab;
        //}
        ////이팩트 생성
        //var effect = Instantiate(targetPrefab1, pos, Quaternion.LookRotation(normal));
        ////if (parent != null)
        //    //effect.transform.SetParent(parent);
        //effect.Play();

        GameObject targetPrefab = CommonHitEffectPooling.GetObjet(pos, normal);
        //만약 생명채가 피격당할 경우이면 사용하는 이펙트를 바꾼다.
        if (effectType == EffectType.Flesh)
        {
            //targetPrefab = fleshHitEffectPrefab;
        }
        StartCoroutine(CommonHitEffectPooling.ReturnObject(targetPrefab));


    }

}