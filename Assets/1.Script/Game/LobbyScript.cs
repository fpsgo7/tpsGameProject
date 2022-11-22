using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyScript : MonoBehaviour
{
    private static LobbyScript instance;

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
    [HideInInspector] public BackEndOut backEndOut;

    public Dropdown dropdown;
    public static int chooseWeapon;
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
    public string name ="Guest";
    public string id;
    public int score;
    public int weaponNum;
    public int equipmentNum;
    public float axisX;
    public float axisY;

    private void Start()
    {
        backEndAuthentication = GetComponent<BackEndAuthentication>();
        backEndGetUserInfo = GetComponent<BackEndGetUserInfo>();
        backEndNickname = GetComponent<BackEndNickname>();
        backEndOut = GetComponent<BackEndOut>();
    }
    public void GameStart()
    {
        chooseWeapon = dropdown.value;
        Debug.Log("게임이 시작됩니다."+chooseWeapon);
        SceneManager.LoadScene("TestCenter");
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
        titleText.GetComponent<Text>().text = name + welcomeName;
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
        titleText.GetComponent<Text>().text = name + welcomeName;
    }

    public void Login()
    {
        backEndAuthentication.Login(LoginIdInput.text,LoginPwInput.text);
    }

    public void Sign()
    {
        backEndAuthentication.Sign(SignIdInput.text, SignPwInput.text);
    }

    public void LogOut()
    {
        backEndOut.LogOut();
    }

    public void SetName()
    {
        if(name == string.Empty)
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
