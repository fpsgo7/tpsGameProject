using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class BackEndAuthentication : MonoBehaviour
{

    //회원가입
    public void Sign(string id, string pw)
    {
        //Backend.BMember.CustomSignUp을 통하여 회원가입 함수를 실행하며
        //결과를 error 변수에 집어넣는다.
        string message
            = Backend.BMember.CustomSignUp(id, pw, "Test1").GetMessage();
        switch (message)
        {
            case "Success":
                Debug.Log("회원가입 성공");
                BackEndPlayerInfo.CreateInsertPlayerInfoData(id);
                Login(id , pw);
                LobbyScript.Instance.backEndGetUserInfo.GetUserInfo();
                LobbyScript.Instance.OpenGameStartPanel();
                break;
            default:
                Debug.Log("중복된 아이디입니다.");
                break;

        }
    }

    public void Login(string id , string pw)
    {
        //로그인 
        string message
            = Backend.BMember.CustomLogin(id, pw).GetMessage();
        switch (message)
        {
            case "Success":
                Debug.Log("로그인 완료 ");
                LobbyScript.Instance.OpenGameStartPanel();
                LobbyScript.Instance.logoutButton.SetActive(true);
                LobbyScript.Instance.GoLoginPanelButton.SetActive(false);
                BackEndPlayerInfo.GetPlayerInfoInLobby(id);
                LobbyScript.Instance.SetTitleText();
                break;
            default:
                Debug.Log("아이디 또는 비번이 틀렸습니다.");
                break;

        }
    }

}
