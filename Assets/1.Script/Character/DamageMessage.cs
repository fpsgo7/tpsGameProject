using UnityEngine;

public struct DamageMessage
{
    public GameObject damager;//공격을 가한측

    public float amount;// 공격 대미지
    public bool isHeadShot;

    public Vector3 hitPoint;//공격 위치
    public Vector3 hitNormal;//  공격이 가해진 반대방향
}