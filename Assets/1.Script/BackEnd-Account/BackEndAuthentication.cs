using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class BackEndAuthentication : MonoBehaviour
{
    //회원가입 로그인 관리
    public LobbyScript lobbyScript;
    public BackEndGetUserInfo backEndGetUserInfo;
    public BackEndNickname backEndNickname;
    public BackEndPlayerInfo backEndPlayerInfo;
    public InputField LoginIdInput;
    public InputField LoginPwInput;
    public InputField JoinIdInput;
    public InputField JoinPwInput;

    public void Start()
    {
        backEndPlayerInfo = GetComponent<BackEndPlayerInfo>();    
    }

    //회원가입
    public void Sign()
    {
        //Backend.BMember.CustomSignUp을 통하여 회원가입 함수를 실행하며
        //결과를 error 변수에 집어넣는다.
        string message 
            = Backend.BMember.CustomSignUp(JoinIdInput.text, JoinPwInput.text, "Test1").GetMessage();
        switch (message)
        {
            case "Success":
                Debug.Log("회원가입 성공");
                Login(JoinIdInput.text, JoinPwInput.text);
                backEndNickname.CreateName();
                backEndGetUserInfo.GetUserInfo();
                lobbyScript.OpenGameStartPanel();
                backEndPlayerInfo.InsertPlayerInfoData(LobbyScript.Instance.id,LobbyScript.Instance.name);
                break;
            default:
                Debug.Log("중복된 아이디입니다.");
                break;

        }
    }

    public void Login()
    {
        //로그인 
        string message 
            = Backend.BMember.CustomLogin(LoginIdInput.text, LoginPwInput.text).GetMessage();
        switch (message)
        {
            //아이디 또는 비번이 틀릴 경우 
            case "Success":
                Debug.Log("로그인 완료 ");
                backEndGetUserInfo.GetUserInfo();
                lobbyScript.OpenGameStartPanel();
                lobbyScript.logoutButton.SetActive(true);
                lobbyScript.GoLoginPanelButton.SetActive(false);
                backEndPlayerInfo.GetPlayerInfo(LoginIdInput.text);
                break;
            default:
                Debug.Log("아이디 또는 비번이 틀렸습니다.");
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
            //아이디 또는 비번이 틀릴 경우 
            case "Success":
                Debug.Log("로그인 완료 ");
                lobbyScript.OpenGameStartPanel();
                lobbyScript.logoutButton.SetActive(true);
                lobbyScript.GoLoginPanelButton.SetActive(false);
                break;
            default:
                Debug.Log("아이디 또는 비번이 틀렸습니다.");
                break;

        }
    }

}
