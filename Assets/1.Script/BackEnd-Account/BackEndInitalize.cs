using UnityEngine;
using BackEnd;

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
