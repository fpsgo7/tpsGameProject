using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
//Json 방식 활용하기
//C:\Users\FPSGO\AppData\LocalLow\DefaultCompany\tpsGameProject
//위 주소로 정보가 저장된다.
//[System.Serializable]
class Player
{
    public string id;
    public string name;
    public int score;
    public int weaponNum;
    public int equipmentNum;
    public float axisX;
    public float axisY;

    public Player(string id,string name, int score, int weaponNum, int equipmentNum, float axisX, float axisY)
    {
        this.id = id;
        this.name = name;
        this.score = score;
        this.weaponNum = weaponNum;
        this.equipmentNum = equipmentNum;
        this.axisX = axisX;
        this.axisY = axisY;
    }
}
public class PlayerInfoManager : MonoBehaviour
{
    private static PlayerInfoManager instance;

    public static PlayerInfoManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<PlayerInfoManager>();

            return instance;
        }
    }

    Player player = new Player("player","name",0,0,0,0,0);
    public string GetPlayerId()
    {
        return player.id;
    }
    string filePath;
    public bool onlineStatus;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        filePath = Application.persistentDataPath + "/PlayerInfo.txt";
    }
    public void SavePlayerScore(int score)
    {

        player.score = score;
        if (onlineStatus)
        {
            BackEndPlayerInfo.SetScoreToServer(player.id, score);
        }
        else
        {
            string jdata = JsonUtility.ToJson(player);
            File.WriteAllText(filePath, jdata);//해당 파일에 입력된다.
            Debug.Log("json 파일에 점수가 저장됩니다.");
        }
    }

    public void ChangePlayerWeapon(int weaponNum)
    {
        player.weaponNum = weaponNum;
        if (onlineStatus)
        {
            BackEndPlayerInfo.SetPlayerWeaponToServer(player.id, weaponNum);
        }
        else
        {
            string jdata = JsonUtility.ToJson(player);
            File.WriteAllText(filePath, jdata);
        }
        
    }

    public void ChangePlayerEquipment(int equipmentNum)
    {
        player.equipmentNum = equipmentNum;
        if (onlineStatus)
        {
            BackEndPlayerInfo.SetPlayerEquipmentToServer(player.id, equipmentNum);
        }
        else
        {
            string jdata = JsonUtility.ToJson(player);
            File.WriteAllText(filePath, jdata);
        }
    }

    public void LoadPlayer()
    {
        if(onlineStatus)
        {
            GameManager.Instance.score = player.score;
            UIManager.Instance.SetScoreText(player.score);
            GameManager.Instance.PlayerStartItem(player.weaponNum, player.equipmentNum);
            GameManager.Instance.PlayerAxisStartSet(player.axisX, player.axisY);
        }
        else if(File.Exists(filePath))
        {
            string jdata = File.ReadAllText(filePath);
            player = JsonUtility.FromJson<Player>(jdata);
            GameManager.Instance.score = player.score;
            UIManager.Instance.SetScoreText(player.score);
            GameManager.Instance.PlayerStartItem(player.weaponNum,player.equipmentNum);
            GameManager.Instance.PlayerAxisStartSet(player.axisX, player.axisY);
        }
    }
    // 마우스 감도 값 넣기
    public void SetAxis(float xAxis, float yAxis)
    {
        player.axisX = xAxis;
        player.axisY = yAxis;
        if (onlineStatus)
        {
            BackEndPlayerInfo.SetAxisToServer(player.id, xAxis, yAxis);
        }
        else
        {
            string jdata = JsonUtility.ToJson(player);
            File.WriteAllText(filePath, jdata);
        }
    }

    //온라인으로 상태값으로 변경하고 받아온값 가져오기
    public void SetOnline(bool onlineStatus,string id,string name,int score, int weaponNum, 
        int equipmentNum, float axisX, float axisY)
    {
        this.onlineStatus = onlineStatus;
        player = new Player(id , name , score, weaponNum, equipmentNum, axisX, axisY);
    }

    
}
