using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndGetUserInfo : MonoBehaviour
{
    //유저정보 받아오기
    public BackEndPlayerInfo backEndPlayerInfo;

    public void Start()
    {
        backEndPlayerInfo = GetComponent<BackEndPlayerInfo>();
    }
    public void GetUserInfo()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();

        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetReturnValue());
            LobbyScript.Instance.name = BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetErrorCode());
        }
    }

}
