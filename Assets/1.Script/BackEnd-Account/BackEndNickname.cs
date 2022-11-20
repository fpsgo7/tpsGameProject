using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using BackEnd;

public class BackEndNickname : MonoBehaviour
{
    //닉네임 생성
    public InputField nickNameInput;
    private bool CheckNickname()
    {
        return Regex.IsMatch(nickNameInput.text, "^[0-9a-zA-Z가-힣]*$");
    }

    // 닉네임 생성
    public void CreateName()
    {
        // 한글, 영어, 숫자로만 닉네임을 만들었는지 체크
        if (CheckNickname() == false)
        {
            Debug.Log("닉네임은 한글, 영어, 숫자로만 만들 수 있습니다.");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.CreateNickname(nickNameInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("닉네임 생성 완료");
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
        }
    }
}
