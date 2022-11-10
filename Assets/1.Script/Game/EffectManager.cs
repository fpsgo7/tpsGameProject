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
    
    //(이팩트 위치, 방향, 이펙트 부모, 사용할 이펙트 타입)
    public void PlayHitEffect(Vector3 pos, Vector3 normal, Transform parent = null, EffectType effectType = EffectType.Common)
    {
        GameObject targetPrefab; 
        //만약 생명채가 피격당할 경우이면 사용하는 이펙트를 바꾼다.
        if (effectType == EffectType.Flesh)
        {
            targetPrefab = FlashHitEffectPooling.GetObjet(pos, normal);
            StartCoroutine(FlashHitEffectPooling.ReturnObject(targetPrefab));
        }
        else
        {
            targetPrefab = CommonHitEffectPooling.GetObjet(pos, normal);
            StartCoroutine(CommonHitEffectPooling.ReturnObject(targetPrefab));
        }
    }
    //폭파함
    public void ExplosionEffect(Transform transform)
    {
        GameObject Explosion;
        Explosion = ExplosionObjectPooling.GetObjet(transform);
        StartCoroutine(ExplosionObjectPooling.ReturnObject(Explosion));
    }

}