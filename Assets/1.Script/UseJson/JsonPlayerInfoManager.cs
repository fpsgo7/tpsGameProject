using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

//C:\Users\FPSGO\AppData\LocalLow\DefaultCompany\tpsGameProject
//위 주소로 정보가 저장된다.
[System.Serializable]
class Player
{
    public string name;
    public int score;
    public string weapon;
    public string equipment;
    public float axisX;
    public float axisY;

    public Player(string name, int score, string weapon, string equipment,float axisX, float axisY)
    {
        this.name = name;
        this.score = score;
        this.weapon = weapon;
        this.equipment = equipment;
        this.axisX = axisX;
        this.axisY = axisY;
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

    Player player = new Player("player",0,string.Empty,string.Empty,0,0);
    string filePath;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        filePath = Application.persistentDataPath+"/PlayerInfo.txt";
    }

    public void SavePlayerScore(int score)
    {

        player.score = score;
        string jdata = JsonUtility.ToJson(player);
        File.WriteAllText(filePath,jdata);//해당 파일에 입력된다.
        Debug.Log("json 파일에 점수가 저장됩니다.");
    }

    public void ChangePlayerWeapon(string weapon)
    {
        player.weapon = weapon;
        string jdata = JsonUtility.ToJson(player);
        File.WriteAllText(filePath, jdata);
    }

    public void ChangePlayerEquipment(string equipment)
    {
        player.equipment = equipment;
        string jdata = JsonUtility.ToJson(player);
        File.WriteAllText(filePath, jdata);
    }

    public void LoadPlayer()
    {
        if(File.Exists(filePath))
        {
            string jdata = File.ReadAllText(filePath);
            player = JsonUtility.FromJson<Player>(jdata);
            GameManager.Instance.score = player.score;
            UIManager.Instance.UpdateScoreText(player.score);
            GameManager.Instance.PlayerStartItem(player.weapon, player.equipment);
            GameManager.Instance.PlayerAxisStartSet(player.axisX, player.axisY);
        }
    }
    // 마우스 감도 값 넣기
    public void GetXaxis(float x)
    {
        player.axisX = x;
        string jdata = JsonUtility.ToJson(player);
        File.WriteAllText(filePath, jdata);
    }
    public void GetYaxis(float y)
    {
        player.axisY = y;
        string jdata = JsonUtility.ToJson(player);
        File.WriteAllText(filePath, jdata);
    }
}
