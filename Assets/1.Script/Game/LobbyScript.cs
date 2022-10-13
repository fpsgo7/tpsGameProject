using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyScript : MonoBehaviour
{
    public Dropdown dropdown;
    public static int chooseWeapon;

    public void GameStart()
    {
        chooseWeapon = dropdown.value;
        Debug.Log("게임이 시작됩니다."+chooseWeapon);
        SceneManager.LoadScene("TestCenter");
    }
}
