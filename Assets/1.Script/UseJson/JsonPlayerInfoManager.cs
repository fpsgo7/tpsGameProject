using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System;
using System.IO;

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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SavePlayerScore(int score)
    {
        PlayerData.Clear();
        PlayerData.Add(new player("player", score));
        string jdata = JsonConvert.SerializeObject(PlayerData);
        File.WriteAllText(Application.dataPath + "/14.Save/PlayerScore.json", jdata);//해당 파일에 입력된다.
        Debug.Log("json 파일에 점수가 저장됩니다.");
    }

    public void LoadPlayerScore()
    {
        if(File.Exists(Application.dataPath + "/14.Save/PlayerScore.json"))
        {
            string jdata = File.ReadAllText(Application.dataPath + "/14.Save/PlayerScore.json");
            PlayerData = JsonConvert.DeserializeObject<List<player>>(jdata);
            GameManager.Instance.score = PlayerData[0].score;
        }
    }
}
