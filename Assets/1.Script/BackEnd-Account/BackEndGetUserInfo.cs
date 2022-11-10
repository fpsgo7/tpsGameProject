using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndGetUserInfo : MonoBehaviour
{
    public LobbyScript lobbyScript;
    public void GetUserInfo()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();

        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetReturnValue());
            lobbyScript.playerName = BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetErrorCode());
        }
    }

}
