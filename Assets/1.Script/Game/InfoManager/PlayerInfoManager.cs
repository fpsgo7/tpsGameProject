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
/// <summary>
///  플레이어 정보를 관리하는 스크립트로
///  플레이어 아이디 , 이름, 점수, 장착 무기, 장착 장비, 마우스x감도, 마우스 y감도
///  값을 관리 사용한다. 
/// </summary>
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
    private string filePath;
    public bool isOnlineStatus;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
        filePath = Application.persistentDataPath + "/PlayerInfo.txt";
    }
    //오프라인 방식 정보 불러오기
    public void SetOfflineLoadPlayer()
    {
        if (File.Exists(filePath))
        {
            string jdata = File.ReadAllText(filePath);
            player = JsonUtility.FromJson<Player>(jdata);
        }
        else// 게임이 처음 실행되는 경우
        {
            player.id = "player";
            player.name = "name";
            player.score = 0;
            player.weaponNum = 0;
            player.equipmentNum = 0;
            player.axisX = 200;
            player.axisY = 2;
        }
    }
    //온라인 방식 정보 불러오기
    public void SetOnlineLoadPlayer(bool onlineStatus, string id, string name, int score, int weaponNum,
        int equipmentNum, float axisX, float axisY)
    {
        this.isOnlineStatus = onlineStatus;
        player = new Player(id, name, score, weaponNum, equipmentNum, axisX, axisY);
    }
    //게임메니저의 정보수정
    public void SetGameManagerPlayerInfo()
    {
        GameManager.Instance.PlayerStartItem(player.weaponNum, player.equipmentNum);
        GameManager.Instance.PlayerAxisStartSet(player.axisX, player.axisY);
        GameManager.Instance.isOnlineStatus = isOnlineStatus;
        GameManager.Instance.score = player.score;
        UIManager.Instance.SetScoreText(player.score);
    }
    //플레이어 정보 가져오기
    public string GetPlayerID()
    {
        return player.id;
    }
    public void SavePlayerScore(int score)
    {

        player.score = score;
        if (isOnlineStatus)
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
        if (isOnlineStatus)
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
        if (isOnlineStatus)
        {
            BackEndPlayerInfo.SetPlayerEquipmentToServer(player.id, equipmentNum);
        }
        else
        {
            string jdata = JsonUtility.ToJson(player);
            File.WriteAllText(filePath, jdata);
        }
    }

    // 마우스 감도 값 넣기
    public void SetAxis(float xAxis, float yAxis)
    {
        player.axisX = xAxis;
        player.axisY = yAxis;
        if (isOnlineStatus)
        {
            BackEndPlayerInfo.SetAxisToServer(player.id, xAxis, yAxis);
        }
        else
        {
            string jdata = JsonUtility.ToJson(player);
            File.WriteAllText(filePath, jdata);
        }
    }
}
