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

    public Dropdown dropdown;
    public static int chooseWeapon;
    public GameObject titleText;
    public GameObject GameStartPanel;
    public GameObject LoginPanel;
    public GameObject JoinPanel;
    public GameObject logoutButton;
    public GameObject GoLoginPanelButton;

    private const string titleName = "TPS Project";
    private const string welcomeName = " 님 환영합니다.";
    public string name ="Guest";
    public string id;
    public int score;
    public int weaponNum;
    public int equipmentNum;
    public float axisX;
    public float axisY;
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
}
