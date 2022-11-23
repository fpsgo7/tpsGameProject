using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;

public static class BackEndInventory 
{
    public static List<Item> InventoryItemList = new List<Item>();// 아이템 관련 리스트 
    public static void GetPlayerInventoryInLobby()
    {
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
                Debug.Log(i);
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
}
