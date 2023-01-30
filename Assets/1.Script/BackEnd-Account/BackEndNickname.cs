using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using BackEnd;
/// <summary>
/// 서버의 닉네임의 중복체크 닉네임 생성
/// 닉네임 수정하는 기능을 담당한다.
/// </summary>
public class BackEndNickname : MonoBehaviour
{
    
    private bool CheckNicknameCollect(string name)
    {
        return Regex.IsMatch(name, "^[0-9a-zA-Z가-힣]*$");
    }
    // 닉네임  중복 체크
    public bool CheckNickname()
    {
       

        BackendReturnObject bro = Backend.BMember.CheckNicknameDuplication(name);
        if (bro.IsSuccess())
        {
            Debug.Log("해당 닉네임으로 수정 가능합니다");
            return true;
        }
        return false;
    }

    // 닉네임 생성
    public bool CreateName(string name)
    {
        // 한글, 영어, 숫자로만 닉네임을 만들었는지 체크
        if (CheckNicknameCollect(name) == false)
        {
            Debug.Log("닉네임은 한글, 영어, 숫자로만 만들 수 있습니다.");
            return false;
        }

        BackendReturnObject BRO = Backend.BMember.CreateNickname(name);

        if (BRO.IsSuccess())
        {
            Debug.Log("닉네임 생성 완료");

            return true;
        }
        else// 실패했을경우
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("이미 중복된 닉네임이 있는 경우");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20자 이상의 닉네임인 경우");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("닉네임에 앞/뒤 공백이 있는경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetErrorCode());
                    break;
            }
            return false;
        }
    }
    //닉네임 수정
    public bool UpdateName(string name)
    {
        // 한글, 영어, 숫자로만 닉네임을 만들었는지 체크
        if (CheckNicknameCollect(name) == false)
        {
            Debug.Log("닉네임은 한글, 영어, 숫자로만 만들 수 있습니다.");
            return false;
        }

        BackendReturnObject BRO = Backend.BMember.UpdateNickname(name);

        if (BRO.IsSuccess())
        {
            Debug.Log("닉네임 생성 완료");

            return true;
        }
        else// 실패했을경우
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("이미 중복된 닉네임이 있는 경우");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20자 이상의 닉네임인 경우");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("닉네임에 앞/뒤 공백이 있는경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetErrorCode());
                    break;
            }
            return false;
        }
    }
}
