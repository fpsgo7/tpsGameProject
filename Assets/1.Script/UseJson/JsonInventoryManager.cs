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
    public string type, name, weaponType, damage, shield;
    public bool isUsing;

    public Item(string type, string name, string weaponType, string damage, string shield, bool isUsing)
    {
        this.type = type;
        this.name = name;
        this.weaponType = weaponType;
        this.damage = damage;
        this.shield = shield;
        this.isUsing = isUsing;
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
    string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/MyItem.txt";// 파일의 경로를 미리 지정함
        Load();
    }

    public void Load()
    {
        if (!File.Exists(filePath)) { return; }
        string jdata = File.ReadAllText(filePath);
        MyItemList = JsonUtility.FromJson<Serialization<Item>>(jdata).target;
        for(int j = 0; j<MyItemList.Count; j++)
        {
            Item UsingItem = MyItemList[j];
            Debug.Log(UsingItem.type);
        }
       
    }

    public void Save(string type, string name, string weaponType, string damage, string shield, bool isUsing)
    {
        MyItemList.Add(new Item(type,name,weaponType,damage,shield,isUsing));
        string jdata = JsonUtility.ToJson(new Serialization<Item>(MyItemList));
        File.WriteAllText(filePath, jdata);//해당 파일에 입력된다.
    }
}
