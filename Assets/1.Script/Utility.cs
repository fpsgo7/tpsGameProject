using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 다른 스크립트에서 수학관련 계산을 위해 사용된다.
/// </summary>
public static class Utility
{
    public static Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance, int areaMask)
    {
        var randomPos = Random.insideUnitSphere * distance + center;
        
        NavMeshHit hit;
        
        NavMesh.SamplePosition(randomPos, out hit, distance, areaMask);
        
        return hit.position;
    }
    //표준편차를 구하기위한 함수
    public static float GedRandomNormalDistribution(float mean, float standard)
    {
        var x1 = Random.Range(0f, 1f);
        var x2 = Random.Range(0f, 1f);
        return mean + standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }
}