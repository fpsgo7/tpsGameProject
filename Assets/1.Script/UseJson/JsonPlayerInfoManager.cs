using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

//C:\Users\FPSGO\AppData\LocalLow\DefaultCompany\tpsGameProject
//위 주소로 정보가 저장된다.
[System.Serializable]
class player
{
    public string name;
    public int score;

    public player(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
public class JsonPlayerInfoManager : MonoBehaviour
{
    private static JsonPlayerInfoManager instance;

    public static JsonPlayerInfoManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<JsonPlayerInfoManager>();

            return instance;
        }
    }

    List<player> PlayerData = new List<player>();
    string filePath;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        filePath = Application.persistentDataPath+"/PlayerInfo.txt";
    }

    public void SavePlayerScore(int score)
    {
        PlayerData.Clear();
        PlayerData.Add(new player("player", score));
        string jdata = JsonUtility.ToJson(PlayerData[0]);
        File.WriteAllText(filePath,jdata);//해당 파일에 입력된다.
        Debug.Log("json 파일에 점수가 저장됩니다.");
    }

    public void LoadPlayerScore()
    {
        if(File.Exists(filePath))
        {
            Debug.Log("파일이 존재합니다.");
            string jdata = File.ReadAllText(filePath);
            GameManager.Instance.score = JsonUtility.FromJson<player>(jdata).score;
        }
    }
}
