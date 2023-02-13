using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 로비의 기능을 전체적으로 관리하는 클래스이다.
/// </summary>
public class LobbyScript : MonoBehaviour
{
    private static LobbyScript instance;
    private InventoryManager inventoryManager;

    public static LobbyScript Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<LobbyScript>();

            return instance;
        }
    }

    [HideInInspector] public BackEndAuthentication backEndAuthentication;
    [HideInInspector] public BackEndGetUserInfo backEndGetUserInfo;
    [HideInInspector] public BackEndNickname backEndNickname;
    private KeySettingInfoManager keySettingInfoManager;
    //public Dropdown dropdown;
    //public static int chooseWeapon;
    public GameObject titleText;
    public GameObject GameStartPanel;
    public GameObject LoginPanel;
    public GameObject JoinPanel;
    public GameObject logoutButton;
    public GameObject GoLoginPanelButton;
    public Text SetNickNameResultText;
    //닉네임 입력 필드
    public InputField nameInputField;
    //아이디 비번 입력 필드
    public InputField LoginIdInput;
    public InputField LoginPwInput;
    public InputField SignIdInput;
    public InputField SignPwInput;

    private const string titleName = "TPS Project";
    private const string welcomeName = " 님 환영합니다.";
    private const string setNickNameFaild = "Faild";
    private const string setNickNameSuccess = "Success";
    public string playerName ="Guest";
    public string id;
    public int score;
    public int weaponNum;
    public int equipmentNum;
    public float xAxis;
    public float yAxis;

    
    private void Start()
    {
        backEndAuthentication = GetComponent<BackEndAuthentication>();
        backEndGetUserInfo = GetComponent<BackEndGetUserInfo>();
        backEndNickname = GetComponent<BackEndNickname>();
        inventoryManager = GameObject.Find("InfoManager").GetComponent<InventoryManager>();
        keySettingInfoManager = GameObject.Find("InfoManager").GetComponent<KeySettingInfoManager>();
    }
    public void GameStart()
    {
        //chooseWeapon = dropdown.value;
        //Debug.Log("게임이 시작됩니다."+chooseWeapon);
        //SceneManager.LoadScene("TestCenter");
        SceneManager.LoadScene("MainGame");
    }

    public void OpenGameStartPanel()
    {
        if (LoginPanel.activeSelf == true)
        {
            GameStartPanel.SetActive(true);
            LoginPanel.SetActive(false);
        }
        if (JoinPanel.activeSelf == true)
        {
            GameStartPanel.SetActive(true);
            JoinPanel.SetActive(false);
        }
        titleText.GetComponent<Text>().text = playerName + welcomeName;
    }

    public void OpenLoginPanel()
    {
        if (GameStartPanel.activeSelf == true)
        {
            GameStartPanel.SetActive(false);
            LoginPanel.SetActive(true);
        }
        if(JoinPanel.activeSelf == true)
        {
            JoinPanel.SetActive(false);
            LoginPanel.SetActive(true);
        }
        titleText.GetComponent<Text>().text = titleName;
    }

    public void OpenJoinPanel()
    {
        if(LoginPanel.activeSelf == true)
        {
            LoginPanel.SetActive(false);
            JoinPanel.SetActive(true);
        }
    }

    public void SetTitleText()
    {
        titleText.GetComponent<Text>().text = playerName + welcomeName;
    }
    //온라인 접속
    public void Login()
    {
        backEndAuthentication.Login(LoginIdInput.text,LoginPwInput.text);
    }
    //오프라인 접속
    public void OfflineLogin()
    {
        PlayerInfoManager.Instance.SetOfflineLoadPlayer();
        inventoryManager.Load();
        keySettingInfoManager.KeySettingLoad();
        OpenGameStartPanel();
    }
    public void Sign()
    {
        backEndAuthentication.Sign(SignIdInput.text, SignPwInput.text);
    }

    public void LogOut()
    {
        BackEndAuthentication.LogOut();
        GoLoginPanelButton.SetActive(true);
        logoutButton.SetActive(false);

        id = string.Empty;
        playerName = string.Empty;
        score = 0;
        weaponNum = 0;
        equipmentNum = 0;
        xAxis = 200;
        yAxis = 2;
        PlayerInfoManager.Instance.SetOnlineLoadPlayer(false, id, playerName, score, weaponNum,
                equipmentNum, xAxis, yAxis);
    }

    public void SetName()
    {
        if(playerName == string.Empty)
        {
            Debug.Log("이름이 없습니다.");
            if (backEndNickname.CreateName(nameInputField.text))
            {
                SetNickNameResultText.text = setNickNameSuccess;
                
                BackEndPlayerInfo.SetNickName(id, nameInputField.text);
            }
            else
            {
                SetNickNameResultText.text = setNickNameFaild;
            }
        }
        else
        {
            if (backEndNickname.UpdateName(nameInputField.text))
            {
                SetNickNameResultText.text = setNickNameSuccess;
                BackEndPlayerInfo.SetNickName(id, nameInputField.text);
            }
            else
            {
                SetNickNameResultText.text = setNickNameFaild;
            }
        }
    }
}
