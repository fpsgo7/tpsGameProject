﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Serialization<T>//T 에는 어떤 형식이든 넣을 수 있어 T를 사용하 였다.
{
    public Serialization(List<T> _target) => target = _target;
    public List<T> target;
}

[System.Serializable]
public class Item
{
   
    public int num;
    public string type, name, weaponType, damage, shield;

    public Item(int num, string type, string name, string weaponType, string damage, string shield)
    {
        this.num = num;
        this.type = type;
        this.name = name;
        this.weaponType = weaponType;
        this.damage = damage;
        this.shield = shield;
    }
}

public class InventoryManager : MonoBehaviour
{
    public List<Item> MyItemList;// 아이템 관련 리스트 
    public string jdata = string.Empty;

    private string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/MyItem.txt";// 파일의 경로를 미리 지정함
    }
    //로그인하거나 , 오프라인 형식으로 시작할때 불러지도록하게한다.
    public void Load()
    {
        if (PlayerInfoManager.Instance.isOnlineStatus)
        {
            Debug.Log("서버에서 데이터를 가져와 덮어 씌웁니다..");
            BackEndInventory.GetPlayerInventoryInLobby();
            MyItemList = BackEndInventory.InventoryItemList;
        }
        else if (File.Exists(filePath)) {
            Debug.Log("오프라인에서 가져옵니다.");
            jdata = File.ReadAllText(filePath);
            MyItemList = JsonUtility.FromJson<Serialization<Item>>(jdata).target;
        }
    }
    //아이템 획득
    public void AddItemSave(int num,string type, string name, string weaponType, string damage, string shield)
    {
        MyItemList.Add(new Item(num,type,name,weaponType,damage,shield));
        if (PlayerInfoManager.Instance.isOnlineStatus)
        {
            BackEndInventory.InsertItem(num, type, name, weaponType, damage, shield);
        }
        else
        {
            jdata = JsonUtility.ToJson(new Serialization<Item>(MyItemList));
            File.WriteAllText(filePath, jdata);//해당 파일에 입력된다.
        }
    }

    public void DeleteItemSave(string jdata, int num)
    {   
        // Inventory 의 MyItemList의 변화된 값을 여기도 적용시켜 똑같이 한다.
        this.jdata = jdata;
        MyItemList = JsonUtility.FromJson<Serialization<Item>>(jdata).target;
        if (PlayerInfoManager.Instance.isOnlineStatus)
        {
            BackEndInventory.DeleteItem(num);
        }
        else
        {
            File.WriteAllText(filePath, jdata);
        }
    }
}
