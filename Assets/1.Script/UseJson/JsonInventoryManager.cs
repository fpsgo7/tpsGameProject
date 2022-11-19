using System.Collections;
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

    public Item(int num, string type, string name, string weaponType, string damage, string shield, bool isUsing)
    {
        this.num = num;
        this.type = type;
        this.name = name;
        this.weaponType = weaponType;
        this.damage = damage;
        this.shield = shield;
    }
}

public class JsonInventoryManager : MonoBehaviour
{
    private static JsonInventoryManager instance;

    public static JsonInventoryManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<JsonInventoryManager>();

            return instance;
        }
    }
    public List<Item> MyItemList;// 아이템 관련 리스트 
    public string jdata = string.Empty;

    private string filePath;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/MyItem.txt";// 파일의 경로를 미리 지정함
        Load();
    }

    public void Load()
    {
        if (!File.Exists(filePath)) { return; }
        jdata = File.ReadAllText(filePath);
        MyItemList = JsonUtility.FromJson<Serialization<Item>>(jdata).target;
    }

    public void AddItemSave(string type, string name, string weaponType, string damage, string shield, bool isUsing)
    {
        MyItemList.Add(new Item(MyItemList.Count+1,type,name,weaponType,damage,shield,isUsing));
        jdata = JsonUtility.ToJson(new Serialization<Item>(MyItemList));
        File.WriteAllText(filePath, jdata);//해당 파일에 입력된다.
    }

    public void DeleteItemSave(string jdata)
    {
        File.WriteAllText(filePath,jdata);
        // Inventory 의 MyItemList의 변화된 값을 여기도 적용시켜 똑같이 한다.
        this.jdata = jdata;
        MyItemList = JsonUtility.FromJson<Serialization<Item>>(jdata).target;
    }
}
