using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class BackEndOut : MonoBehaviour
{
   public void LogOut()
    {
        BackendReturnObject BRO = Backend.BMember.Logout();
        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetMessage());
            LobbyScript.Instance.GoLoginPanelButton.SetActive(true);
            LobbyScript.Instance.logoutButton.SetActive(false);

            LobbyScript.Instance.id = string.Empty;
            LobbyScript.Instance.name = string.Empty;
            LobbyScript.Instance.score = 0;
            LobbyScript.Instance.weaponNum = 0;
            LobbyScript.Instance.equipmentNum = 0;
            LobbyScript.Instance.axisX = 200;
            LobbyScript.Instance.axisY = 2;

            JsonPlayerInfoManager.Instance.SetOnline(false,LobbyScript.Instance.id, LobbyScript.Instance.name,
                LobbyScript.Instance.score, LobbyScript.Instance.weaponNum,
                LobbyScript.Instance.equipmentNum, LobbyScript.Instance.axisX,
                LobbyScript.Instance.axisY);
        }
        else
        {
            Debug.Log(BRO.GetMessage());
        }
    }
}
