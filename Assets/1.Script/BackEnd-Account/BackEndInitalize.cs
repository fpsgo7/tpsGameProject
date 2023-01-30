using UnityEngine;
using BackEnd;
/// <summary>
/// 벡엔드 관련 기본작업을 하는 클래스이다.
/// </summary>
public class BackEndInitalize : MonoBehaviour
{
    // 백엔드 기본작업
    //.Net4 버전
    void Awake()
    {
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공!");
        }
        else
        {
            Debug.LogError("초기화 실패");
        }
    }
}
