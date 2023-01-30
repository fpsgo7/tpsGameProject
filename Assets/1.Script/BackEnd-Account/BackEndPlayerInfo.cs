using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
/// <summary>
/// 플레이어의 아이디 이름 점수 장착 아이템등 정보를
/// 서버로부터 가져오거나 수정하거나
/// 회원가입할때 생성하는 기능을한다.
/// </summary>
public static class BackEndPlayerInfo
{
    // 플레이어의 정보를 가져온다.
    public static void GetPlayerInfo(string id)
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
        //로비스크립트에서 정보를 활용하기위해 저장함
        LobbyScript.Instance.id = bro.FlattenRows()[0]["id"].ToString();
        LobbyScript.Instance.name = bro.FlattenRows()[0]["name"].ToString();
        LobbyScript.Instance.score = Convert.ToInt32(bro.FlattenRows()[0]["score"].ToString());
        LobbyScript.Instance.weaponNum = Convert.ToInt32(bro.FlattenRows()[0]["weaponNum"].ToString());
        LobbyScript.Instance.equipmentNum = Convert.ToInt32(bro.FlattenRows()[0]["equipmentNum"].ToString());
        LobbyScript.Instance.xAxis = float.Parse(bro.FlattenRows()[0]["xAxis"].ToString());
        LobbyScript.Instance.yAxis = float.Parse(bro.FlattenRows()[0]["yAxis"].ToString());
        LobbyScript.Instance.SetTitleText();
        //플레이어 인포메니저는 파괴되지 않는 오브젝트에 값을 넣어줘 다음씬에서 사용할 수 있게한다
        PlayerInfoManager.Instance.SetOnlineLoadPlayer(true,LobbyScript.Instance.id, LobbyScript.Instance.name,
            LobbyScript.Instance.score, LobbyScript.Instance.weaponNum,
            LobbyScript.Instance.equipmentNum, LobbyScript.Instance.xAxis,
            LobbyScript.Instance.yAxis);
    }
    // 회원가입할때 서버에 정보를 추가한다. 
    public static void InsertPlayerInfoData(string id)
    {

        // Param은 뒤끝 서버와 통신을 할 떄 넘겨주는 파라미터 클래스 입니다. 
        Param param = new Param();
        //int 형에서 string 형으로 자동 변환 이 되지않아 문자열 형태로 교체후 사용할 것
        param.Add("id",id);
        param.Add("score",0);
        param.Add("weaponNum", 0);
        param.Add("equipmentNum", 0);
        param.Add("xAxis",200);
        param.Add("yAxis",2);

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
    // 회원의 이름을 수정한다.
    public static void SetNickName(string id, string name)
    {
        Where where = new Where();
        where.Equal("id",id);

        Param param = new Param();
        param.Add("name", name);

        var bro = Backend.GameData.Update("PlayerInfo", where, param);
      
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
    }
    // 회원의 마우스 감도값을 수정한다.
    public static void SetAxisToServer(string id, float xAxis, float yAxis)
    {
        Where where = new Where();
        where.Equal("id", id);

        Param param = new Param();
        param.Add("xAxis", xAxis);
        param.Add("yAxis", yAxis);

        var bro = Backend.GameData.Update("PlayerInfo", where, param);

        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log("실패" + bro);
            return;
        }
        else
        {
            Debug.Log("성공" + bro);
        }
    }
    // 플레이어의 점수를 서버에 적용하기
    public static void SetScoreToServer(string id, int score)
    {
        Where where = new Where();
        where.Equal("id", id);

        Param param = new Param();
        param.Add("score", score);

        var bro = Backend.GameData.Update("PlayerInfo", where, param);

        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log("실패" + bro);
            return;
        }
        else
        {
            Debug.Log("성공" + bro);
        }
    }
    // 플레이어가 장착한 무기를 서버에 적용하기
    public static void SetPlayerWeaponToServer(string id, int weaponNum)
    {
        Where where = new Where();
        where.Equal("id", id);

        Param param = new Param();
        param.Add("weaponNum", weaponNum);

        var bro = Backend.GameData.Update("PlayerInfo", where, param);

        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log("실패" + bro);
            return;
        }
        else
        {
            Debug.Log("성공" + bro);
        }
    }
    // 플레이어가 장착한 장비를 서버에 적용하기
    public static void SetPlayerEquipmentToServer(string id, int equipmentNum)
    {
        Where where = new Where();
        where.Equal("id", id);

        Param param = new Param();
        param.Add("equipmentNum", equipmentNum);

        var bro = Backend.GameData.Update("PlayerInfo", where, param);

        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log("실패" + bro);
            return;
        }
        else
        {
            Debug.Log("성공" + bro);
        }
    }
}
