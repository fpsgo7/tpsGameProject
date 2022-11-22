﻿using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;

public class BackEndPlayerInfo : MonoBehaviour
{
    public void GetPlayerInfo(string id)
    {
        Where where = new Where();
        where.Equal("id",id);
        
        var bro = Backend.GameData.Get("PlayerInfo", where, 10);
      
        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log("실패"+bro);
            return;
        }
        else
        {
            Debug.Log("성공"+bro);
        }

        LobbyScript.Instance.id = bro.FlattenRows()[0]["id"].ToString();
        LobbyScript.Instance.name = bro.FlattenRows()[0]["name"].ToString();
        LobbyScript.Instance.score = Convert.ToInt32(bro.FlattenRows()[0]["score"].ToString());
        LobbyScript.Instance.weaponNum = Convert.ToInt32(bro.FlattenRows()[0]["weaponNum"].ToString());
        LobbyScript.Instance.equipmentNum = Convert.ToInt32(bro.FlattenRows()[0]["equipmentNum"].ToString());
        LobbyScript.Instance.axisX = float.Parse(bro.FlattenRows()[0]["axisX"].ToString());
        LobbyScript.Instance.axisY = float.Parse(bro.FlattenRows()[0]["axisY"].ToString());
       

        JsonPlayerInfoManager.Instance.SetOnline(true,LobbyScript.Instance.id, LobbyScript.Instance.name,
            LobbyScript.Instance.score, LobbyScript.Instance.weaponNum,
            LobbyScript.Instance.equipmentNum, LobbyScript.Instance.axisX,
            LobbyScript.Instance.axisY);
    }
    // Insert 는 '생성' 작업에 주로 사용된다. 
    public void InsertPlayerInfoData(string id)
    {

        // Param은 뒤끝 서버와 통신을 할 떄 넘겨주는 파라미터 클래스 입니다. 
        Param param = new Param();
        //int 형에서 string 형으로 자동 변환 이 되지않아 문자열 형태로 교체후 사용할 것
        param.Add("id",id);
        param.Add("score",0);
        param.Add("axisX",200);
        param.Add("axisY",2);

        BackendReturnObject BRO = Backend.GameData.Insert("PlayerInfo", param);

        if (BRO.IsSuccess())
        {
            Debug.Log("indate : " + BRO.GetInDate());
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                    break;
            }
        }
    }

    public void SetNickName()
    {

    }
}