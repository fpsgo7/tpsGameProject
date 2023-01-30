using UnityEngine;
/// <summary>
/// 적의 데미지와 데미지 표현을 위한 변수들을 담아 클래스간 
/// 이동에 사용되는 클래스이다.
/// </summary>
public struct DamageMessage
{
    public LivingEntity damagerLivingEntity;//공격을 가한측

    public float amount;// 공격 대미지
    public bool isHeadShot;

    public Vector3 hitPoint;//공격 위치
    public Vector3 hitNormal;//  공격이 가해진 반대방향
}