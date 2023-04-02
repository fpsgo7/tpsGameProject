using UnityEngine;
using System.IO;
//Json 방식 활용하기
//C:\Users\FPSGO\AppData\LocalLow\DefaultCompany\tpsGameProject
//위 주소로 정보가 저장된다.
//[System.Serializable]

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

    public PlayerInfo playerInfo = new PlayerInfo(string.Empty,string.Empty,0,0,0,0,0);// 플렝이ㅓ개체 생성
    private string filePath;
    public bool isOnlineStatus;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
        filePath = Application.persistentDataPath + "/PlayerInfo.txt";
    }
    //오프라인 방식 정보 불러오기
    public void SetOfflineLoadPlayerInfo()
    {
        if (File.Exists(filePath))
        {
            string jdata = File.ReadAllText(filePath);
            playerInfo = JsonUtility.FromJson<PlayerInfo>(jdata);
        }
        else// 게임이 처음 실행되는 경우
        {
            playerInfo.id = "player";
            playerInfo.name = "name";
            playerInfo.score = 0;
            playerInfo.weaponNum = 0;
            playerInfo.equipmentNum = 0;
            playerInfo.axisX = 200;
            playerInfo.axisY = 2;
        }
    }
    //온라인 방식 정보 불러오기
    public void SetOnlineLoadPlayerInfo(bool onlineStatus, string id, string name, int score, int weaponNum,
        int equipmentNum, float axisX, float axisY)
    {
        this.isOnlineStatus = onlineStatus;
        playerInfo.SetPlayerInfo(id, name, score, weaponNum, equipmentNum, axisX, axisY);
    }
    //로그아웃 하기 플레이어 정보 비우기
    public void ResetPlayerInfo()
    {
        this.isOnlineStatus = false;
        playerInfo.SetPlayerInfo(string.Empty, string.Empty, 0, 0, 0, 0, 0);
    }
    //게임메니저의 정보수정
    public void SetGameManagerPlayerInfo()
    {
        GameManager.Instance.PlayerStartItem(playerInfo.weaponNum, playerInfo.equipmentNum);
        GameManager.Instance.PlayerAxisStartSet(playerInfo.axisX, playerInfo.axisY);
        GameManager.Instance.isOnlineStatus = isOnlineStatus;
        GameManager.Instance.score = playerInfo.score;
        UIManager.Instance.SetScoreText(playerInfo.score);
    }
    //플레이어 정보 가져오기
    public string GetPlayerID()
    {
        return playerInfo.id;
    }
    public void SavePlayerScore(int score)
    {

        playerInfo.score = score;
        if (isOnlineStatus)
        {
            BackEndPlayerInfo.SetScoreToServer(playerInfo.id, score);
        }
        else
        {
            string jdata = JsonUtility.ToJson(playerInfo);
            File.WriteAllText(filePath, jdata);//해당 파일에 입력된다.
            Debug.Log("json 파일에 점수가 저장됩니다.");
        }
    }

    public void ChangePlayerWeapon(int weaponNum)
    {
        playerInfo.weaponNum = weaponNum;
        if (isOnlineStatus)
        {
            BackEndPlayerInfo.SetPlayerWeaponToServer(playerInfo.id, weaponNum);
        }
        else
        {
            string jdata = JsonUtility.ToJson(playerInfo);
            File.WriteAllText(filePath, jdata);
        }
        
    }

    public void ChangePlayerEquipment(int equipmentNum)
    {
        playerInfo.equipmentNum = equipmentNum;
        if (isOnlineStatus)
        {
            BackEndPlayerInfo.SetPlayerEquipmentToServer(playerInfo.id, equipmentNum);
        }
        else
        {
            string jdata = JsonUtility.ToJson(playerInfo);
            File.WriteAllText(filePath, jdata);
        }
    }

    // 마우스 감도 값 넣기
    public void SetAxis(float xAxis, float yAxis)
    {
        playerInfo.axisX = xAxis;
        playerInfo.axisY = yAxis;
        if (isOnlineStatus)
        {
            BackEndPlayerInfo.SetAxisToServer(playerInfo.id, xAxis, yAxis);
        }
        else
        {
            string jdata = JsonUtility.ToJson(playerInfo);
            File.WriteAllText(filePath, jdata);
        }
    }
}
