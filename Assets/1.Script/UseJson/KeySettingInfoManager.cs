using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
//바이너리 방식으로 저장하기
//저장 루트 
//C:\Users\FPSGO\AppData\LocalLow\DefaultCompany\tpsGameProject
[System.Serializable]
public class KeySettingData
{
    public int FIRE;
    public int ZOOMIN;
    public int GRENADE;
    public int JUMP;
    public int RELOAD;
    public int RESTOREHEALTH;
    public int SCOPEZOOMIN;
    public int CANCEL;
    public int INVENTORY;
    public int INTERACTION;
    public int RUN;
}
public class KeySettingInfoManager : MonoBehaviour
{
    public KeySettingData keySettingData;
    KeyCode[] defaultKeys = new KeyCode[]
       {KeyCode.Mouse0,KeyCode.Mouse1,KeyCode.G,KeyCode.Space,KeyCode.R,KeyCode.V,
        KeyCode.Tab,KeyCode.Escape,KeyCode.M,KeyCode.E,KeyCode.LeftShift};

    private void OnEnable()
    {
        KeySetting.keys.Clear();
        keySettingData = KeySettingLoad();
    }
    public void KeySettingSave(KeySettingData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "KeySettingInfo.bin");
        FileStream stream = File.Create(path);
        formatter.Serialize(stream,data);
        stream.Close();
    }

   public KeySettingData KeySettingLoad()
   {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath, "KeySettingInfo.bin");
            FileStream stream = File.OpenRead(path);
            KeySettingData keySettingData = (KeySettingData)formatter.Deserialize(stream);
            stream.Close();
            return keySettingData;
        }
        catch(Exception e)
        {
            KeySettingData keySettingData = new KeySettingData();
            keySettingData.FIRE = (int)defaultKeys[0];
            keySettingData.ZOOMIN = (int)defaultKeys[1];
            keySettingData.GRENADE = (int)defaultKeys[2];
            keySettingData.JUMP = (int)defaultKeys[3];
            keySettingData.RELOAD = (int)defaultKeys[4];
            keySettingData.RESTOREHEALTH = (int)defaultKeys[5];
            keySettingData.SCOPEZOOMIN = (int)defaultKeys[6];
            keySettingData.CANCEL = (int)defaultKeys[7];
            keySettingData.INVENTORY = (int)defaultKeys[8];
            keySettingData.INTERACTION = (int)defaultKeys[9];
            keySettingData.RUN = (int)defaultKeys[10];
            return keySettingData;
        }
        
   }
}
