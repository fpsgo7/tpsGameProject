using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

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
            try
            {
                LobbyScript.Instance.name = BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString();
            }
            catch(Exception e){
                Debug.Log("닉네임이 없습니다.");
                LobbyScript.Instance.name = string.Empty;
            }
            
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetErrorCode());
        }
    }

}
