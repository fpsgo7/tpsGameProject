using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;

public class BackEndPlayerInfo : MonoBehaviour
{
    public string id;
    public string name;
    public int score;
    public int weaponNum;
    public int equipmentNum;
    public float axisX;
    public float axisY;
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

        id = bro.FlattenRows()[0]["id"].ToString();
        name = bro.FlattenRows()[0]["name"].ToString();
        score = Convert.ToInt32( bro.FlattenRows()[0]["score"].ToString());
        weaponNum = Convert.ToInt32( bro.FlattenRows()[0]["weaponNum"].ToString());
        equipmentNum  = Convert.ToInt32(bro.FlattenRows()[0]["equipmentNum"].ToString());
        axisX = float.Parse(bro.FlattenRows()[0]["axisX"].ToString());
        axisY = float.Parse(bro.FlattenRows()[0]["axisY"].ToString()); 
        JsonPlayerInfoManager.Instance.SetOnline(id, name, score, weaponNum, equipmentNum, axisX, axisY);
    }
    // Insert 는 '생성' 작업에 주로 사용된다. 
    public void InsertPlayerInfoData(string id,string name)
    {

        // Param은 뒤끝 서버와 통신을 할 떄 넘겨주는 파라미터 클래스 입니다. 
        Param param = new Param();
        //int 형에서 string 형으로 자동 변환 이 되지않아 문자열 형태로 교체후 사용할 것
        param.Add("id",id);
        param.Add("name",name);
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
}
