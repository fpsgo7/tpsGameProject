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
    public int INVENTORY;
    public int INTERACTION;
    public int RUN;
}
/// <summary>
/// 키 설정을 관리하는 클래스로
/// 키값을 파일 또는 서버연동 스크립트를 통해  
/// 가져오거나 저장한다.
/// </summary>
public class KeySettingInfoManager : MonoBehaviour
{
    //다른 스크립트에서도 사용할 수 있는 클래스형 변수 생성
    public KeySettingData keySettingData;
    KeyCode[] defaultKeys = new KeyCode[]
       {KeyCode.Mouse0,KeyCode.Mouse1,KeyCode.G,KeyCode.Space,KeyCode.R,KeyCode.V,
        KeyCode.Tab,KeyCode.M,KeyCode.E,KeyCode.LeftShift};

    public void KeySettingLoad()
    {
        Debug.Log("청소");
        KeySetting.keys.Clear();

        try
        {
            if (PlayerInfoManager.Instance.isOnlineStatus)
            {
                keySettingData = BackEndKeySetting.GetPlayerKeySetting();
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                string path = Path.Combine(Application.persistentDataPath, "KeySettingInfo.bin");
                FileStream stream = File.OpenRead(path);
                keySettingData = (KeySettingData)formatter.Deserialize(stream);
                stream.Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            keySettingData.FIRE = (int)defaultKeys[0];
            keySettingData.ZOOMIN = (int)defaultKeys[1];
            keySettingData.GRENADE = (int)defaultKeys[2];
            keySettingData.JUMP = (int)defaultKeys[3];
            keySettingData.RELOAD = (int)defaultKeys[4];
            keySettingData.RESTOREHEALTH = (int)defaultKeys[5];
            keySettingData.SCOPEZOOMIN = (int)defaultKeys[6];
            keySettingData.INVENTORY = (int)defaultKeys[7];
            keySettingData.INTERACTION = (int)defaultKeys[8];
            keySettingData.RUN = (int)defaultKeys[9];
        }
    }
    public void KeySettingSave(KeySettingData data)
    {
        if (PlayerInfoManager.Instance.isOnlineStatus)
        {
            BackEndKeySetting.SetPlayerKeySetting(PlayerInfoManager.Instance.GetPlayerID(),keySettingData);
        }
        else
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath, "KeySettingInfo.bin");
            FileStream stream = File.Create(path);
            formatter.Serialize(stream, data);
            stream.Close();
        }
            
    }
}
