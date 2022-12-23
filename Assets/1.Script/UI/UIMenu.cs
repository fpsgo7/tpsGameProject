using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//2. 무기 창과 장비창 배열로 하여 관리하여 다른 장비창도 들어올 수 있게 설계하기 ***
public class UIMenu : MonoBehaviour
{
    //싱글톤 방식 사용
    private static UIMenu instance;

    public static UIMenu Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIMenu>();

            return instance;
        }
    }

    //각 오브젝트들을 유니티 인스팩터 상에서 연결하기위하여 SerializeField 사용
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject KeyChangeUI;
    [SerializeField] private List<GameObject> EquipmentPanels = new List<GameObject>();
    [SerializeField] private GameObject EquipmentChangeButton;
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private PlayerSubCamera playerSubCamera;
    [HideInInspector] public bool isMenuUI = false;

    //메뉴 오픈
    public void SetMenuOnOff()
    {
        if (isMenuUI == false && MenuUI.activeSelf == false)
        {
            //playerSubCamera.ActiveForrowCamInventory();
            Cursor.lockState = CursorLockMode.Confined;
            isMenuUI = true;
            MenuUI.SetActive(true);
            Cursor.visible = true;
            playerShooter.AxisActiveMenuChange();
            //Debug.Log("메뉴 오픈");
        }
        else if (isMenuUI == true && MenuUI.activeSelf == true)
        {
            //playerSubCamera.InactiveForrowCamInventory();
            isMenuUI = false;
            MenuUI.SetActive(false);
            Cursor.visible = false;
            //조준감도 설정
            playerShooter.XAxisChange(UIAim.Instance.xAxis);
            playerShooter.YAxisChange(UIAim.Instance.yAxis);
            PlayerInfoManager.Instance.SetAxis(UIAim.Instance.xAxis, UIAim.Instance.yAxis);
            playerShooter.AxisMenuInactiveChange();
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
    //인벤토리 오픈
    public void SetInventoryOnOff()
    {
        if (isMenuUI == false && InventoryUI.activeSelf == false)
        {
            playerSubCamera.ActiveForrowCamInventory();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            isMenuUI = true;
            InventoryUI.SetActive(true);
            playerShooter.AxisActiveMenuChange();
            Debug.Log("인벤토리 오픈 오픈");
        }
        else if (isMenuUI == true && InventoryUI.activeSelf == true)
        {
            playerSubCamera.InactiveForrowCamInventory();
            isMenuUI = false;
            InventoryUI.SetActive(false);
            Cursor.visible = false;
            playerShooter.AxisMenuInactiveChange();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    //키보드 변경창 오픈
    public void SetKeyChangeUIOnOff()
    {
        if (KeyChangeUI.activeSelf == false)
        {
            MenuUI.SetActive(false);
            KeyChangeUI.SetActive(true);
        }
        else if (KeyChangeUI.activeSelf == true)
        {
            MenuUI.SetActive(true);
            KeyChangeUI.SetActive(false);
        }
    }
    //장비창 변경
    public void ChangeEquipmentPanel(int EquipmentNum)
    {
        for (int i = 0; i < EquipmentPanels.Count; i++)
        {
            EquipmentPanels[i].SetActive(false);
        }
        EquipmentPanels[EquipmentNum].SetActive(true);
    }
    //로비로 돌아가기
    public void ExitClick()
    {
        if (PlayerInfoManager.Instance.onlineStatus)
        {
            BackEndInventory.InventoryItemList.Clear();// 로그아웃 을했기 때문에 리스트를 초기화 시켜준다.
            BackEndAuthentication.LogOut();// 로그아웃
        }
        SceneManager.LoadScene("Lobby");
    }

}
