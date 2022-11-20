using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class BackEndOut : MonoBehaviour
{
    //로그아웃
    public LobbyScript lobbyScript;
   public void LogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();
        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetMessage());
            lobbyScript.GoLoginPanelButton.SetActive(true);
            lobbyScript.logoutButton.SetActive(false);
        }
        else
        {
            Debug.Log(BRO.GetMessage());
        }
    }
}
