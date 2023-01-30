using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;
/// <summary>
/// 서버로부터 인벤토리를 가져오는 클래스이다.
/// </summary>
public static class BackEndInventory 
{
    public static List<Item> InventoryItemList = new List<Item>();// 아이템 관련 리스트 
    //로비씬에서 서버로부터 플레이어 정보를 가져온다.
    public static void GetPlayerInventoryInLobby()
    {
        //자신의 아이디에 맞는 정보를 가져옴
        var bro = Backend.GameData.GetMyData("PlayerInventory", new Where(), 100);

        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log("실패" + bro);
            return;
        }
        else
        {
            for(int i =0; i< bro.Rows().Count; ++i)
            {
                InventoryItemList.Add(new Item(Convert.ToInt32(bro.FlattenRows()[i]["num"].ToString()),
                    bro.FlattenRows()[i]["type"].ToString(),
                    bro.FlattenRows()[i]["name"].ToString(),
                    bro.FlattenRows()[i]["weaponType"].ToString(),
                    bro.FlattenRows()[i]["damage"].ToString(),
                    bro.FlattenRows()[i]["shield"].ToString()
                    ));
                
            }
            return;
        }
    }
    // 서버에 새로얻은 아이템 정보를 넣는 기능을한다.
    public static void InsertItem(int num, string type, string name, string weaponType, string damage, string shield)
    {
        Param param = new Param();
        param.Add("num", num);
        param.Add("type", type);
        param.Add("name", name);
        param.Add("weaponType", weaponType);
        param.Add("damage", damage);
        param.Add("shield", shield);
        BackendReturnObject BRO = Backend.GameData.Insert("PlayerInventory", param);
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
    //서버의 아이템 삭제
    public static void DeleteItem(int num)
    {
        Where where = new Where();
        where.Equal("num", num);
        BackendReturnObject BRO = Backend.GameData.Delete("PlayerInventory", where);
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

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                    break;
            }
        }
    }
    
}
