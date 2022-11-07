using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//UI 와 마우스 커서를 관리한다.
public class UIManager : MonoBehaviour
{
    //싱글톤 방식 사용
    private static UIManager instance;
    
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();

            return instance;
        }
    }
    //각 오브젝트들을 유니티 인스팩터 상에서 연결하기위하여 SerializeField 사용
    [SerializeField] private GameObject gameoverUI;
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject restoreHealthSlider;
    [SerializeField] private GameObject WeaponPanel;
    [SerializeField] private GameObject EquipmentPanel;
    [SerializeField] private GameObject EquipmentChangeButton;
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Text healthText;
    [SerializeField] private Text lifeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text healthKitText;
    [SerializeField] private Text grenadeText;
    [SerializeField] private Text fullAmmoText;
    [SerializeField] private Text waveText;

    public bool menuUIOpen = false;
    public PlayerShooter playerShooter;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //탄약 업데읻트
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo+"" ;
        fullAmmoText.text = remainAmmo+"";
    }
    //점수 업데이트
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }
    //웨이브 업데이트
    public void UpdateEnemyText(int count)
    {
        waveText.text = "Enemy Left : " + count;
    }
    //체력 업데이트
    public void UpdateLifeText(int count)
    {
        lifeText.text = "Life : " + count;
    }
    //크로스해어 업데이트
    public void UpdateCrossHairPosition(Vector3 worldPosition)
    {
        crosshair.UpdatePosition(worldPosition);
    }
    //최대 체력 슬라이더에 적용
    public void UpdateHealthMaxSlider(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
    }
    //체력을 보여줌
    public void UpdateHealthText(float health)
    {
        healthText.text = Mathf.Floor(health).ToString();
        healthSlider.value = health;
    }
    //체력 회복 킷트
    public void UpdateHealthKitText(int healthKit)
    {
        healthKitText.text = healthKit+"";
    }
    //체력 충전 값 공유
    public void HealthMax(int max)
    {
        restoreHealthSlider.GetComponent<Slider>().maxValue = max;
    }
    //체력 충전 시작
    public void UpdateRestoreHealthStart()
    {
        restoreHealthSlider.SetActive(true);
    }
    //체력 충전 업데이트
    public void UpdateRestoreHealth(int restore)
    {
        restoreHealthSlider.GetComponent<Slider>().value = restore;
    }
    //체력 충전 종료
    public void UpdateRestoreHealthEnd()
    {
        restoreHealthSlider.SetActive(false);
    }
    //수류탄 개수
    public void UpdateGrenadeText(int grenade)
    {
        grenadeText.text = grenade + "";
    }
    //크로스헤어 보여주기 관련 
    public void SetActiveCrosshair(bool active)
    {
        crosshair.SetActiveCrosshair(active);
    }
    //게임오버 관련
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }
    //게임 제시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //메뉴 오픈
    public void MenuOnOff()
    {
        if (menuUIOpen == false && MenuUI.activeSelf == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            menuUIOpen = true;
            MenuUI.SetActive(true);
            Cursor.visible = true;
            //playerShooter.AxisMenuOnChange();
            Debug.Log("메뉴 오픈");
        }
        else if(menuUIOpen == true && MenuUI.activeSelf == true)
        {
            menuUIOpen = false;
            MenuUI.SetActive(false);
            Cursor.visible = false;
            //playerShooter.AxisMenuOffChange();
            Cursor.lockState = CursorLockMode.Locked;
        }
            
    }
    //인벤토리 오픈
    public void InventoryOnOff()
    {
        if (menuUIOpen == false && InventoryUI.activeSelf == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            menuUIOpen = true;
            InventoryUI.SetActive(true);
            Cursor.visible = true;
            //playerShooter.AxisMenuOnChange();
            Debug.Log("인벤토리 오픈 오픈");
        }
        else if(menuUIOpen == true && InventoryUI.activeSelf == true)
        {
            menuUIOpen = false;
            InventoryUI.SetActive(false);
            Cursor.visible = false;
            //playerShooter.AxisMenuOffChange();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    //장비창 변경
    public void ChangeEquipmentPanel()
    {
        if(WeaponPanel.activeSelf == false)
        {
            EquipmentPanel.SetActive(false);
            WeaponPanel.SetActive(true);
            EquipmentChangeButton.GetComponentInChildren<Text>().text = "장비창으로";
        }
        else
        {
            WeaponPanel.SetActive(false);
            EquipmentPanel.SetActive(true);
            EquipmentChangeButton.GetComponentInChildren<Text>().text = "무기창으로";
        }
    }
    //로비로 돌아가기
    public void OnExitClick()
    {
        SceneManager.LoadScene("Lobby");
    }
}