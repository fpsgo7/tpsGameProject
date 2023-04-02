using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
/// <summary>
/// 실제 회원가입  로그인 로그아웃 기능을 서버와 연동하여 수행하며
/// 얻어온 정보를 키 새팅 클래스와 인벤토리 관리 클래스에 넘겨준다.
/// </summary>
public class BackEndAuthentication : MonoBehaviour
{
    private KeySettingInfoManager keySettingInfoManager;
    private InventoryManager inventoryManager;
    private void Awake()
    {
        keySettingInfoManager = GameObject.Find("InfoManager").GetComponent<KeySettingInfoManager>();
        inventoryManager = GameObject.Find("InfoManager").GetComponent<InventoryManager>();
    }
    
    //회원가입
    /// <summary>
    /// 회원가입 기능으로 
    /// 해당 함수가 실행되면 서버에 회원정보를 입력시도하며
    /// 성공하면 회원가입이 되며 
    /// 해당 아이디 값을 통해 플레이어정보도 서버에 새로 추가해주고
    /// 키설정 관련 값도 서버에 아이디값과 함꼐 추가된다.
    /// </summary>
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
                BackEndPlayerInfo.InsertPlayerInfoData(id);
                BackEndKeySetting.InsertPlayerKeySetting(id);
                Login(id , pw);
                break;
            default:
                Debug.Log("중복된 아이디입니다.");
                break;

        }
    }
    /// <summary>
    /// 로그인 기능으로 
    /// 로그인 시도가 성공하면 
    /// 서버에서
    /// 인벤토리 내용을 가져오고
    /// 키설정 값도 가져온다.
    /// </summary>
    public void Login(string id , string pw)
    {
        //로그인 
        string message
            = Backend.BMember.CustomLogin(id, pw).GetMessage();
        switch (message)
        {
            case "Success":
                BackEndPlayerInfo.GetPlayerInfo(id);
                inventoryManager.Load();
                keySettingInfoManager.KeySettingLoad();
                LobbyScript.Instance.OpenGameStartPanel(false);
                LobbyScript.Instance.SetTitleText(PlayerInfoManager.Instance.playerInfo.name);
                break;
            default:
                Debug.Log("아이디 또는 비번이 틀렸습니다.");
                break;

        }
    }
    /// <summary>
    /// 로그아웃 기능을 한다.
    /// </summary>
    public static void LogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();
        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetMessage());
        }
        else
        {
            Debug.Log(BRO.GetMessage());
        }
    }
}
