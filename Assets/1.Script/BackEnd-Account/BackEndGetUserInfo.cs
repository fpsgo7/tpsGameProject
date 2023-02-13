using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;
/// <summary>
/// 유저의 닉네임을 가져오는 클래스
/// </summary>
public class BackEndGetUserInfo : MonoBehaviour
{
    public void GetUserInfo()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();

        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetReturnValue());
            try
            {
                LobbyScript.Instance.playerName = BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString();
            }
            catch(Exception e){
                Debug.Log("닉네임이 없습니다."+e.Message);
                LobbyScript.Instance.playerName = string.Empty;
            }
            
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetErrorCode());
        }
    }

}
