using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//스크립트 수정할것!
//1.getcomponent 등 두번이상 작동하는 대상은 start 함수로 캐싱한다..
//2.함수이름 수정할것 update 나 이런 이름대신 set 활용
//3.UIManager이 너무 많은 기능을 함유하고 있기 때문에 기능을 분리하여 새로운 클래스에 옮길것!
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
    [SerializeField] private GameObject itemBoxOpenText;
    [SerializeField] private GameObject explosionWallText;
    [SerializeField] private GameObject DamageTextParents;
    [SerializeField] private GameObject damageText;
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider XAxisSlider;
    [SerializeField] private Slider YAxisSlider;

    [SerializeField] private Text healthText;
    [SerializeField] private Text lifeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text healthKitText;
    [SerializeField] private Text grenadeText;
    [SerializeField] private Text fullAmmoText;
    [SerializeField] private Text waveText;
    [SerializeField] private Text XAxisText;
    [SerializeField] private Text YAxisText;

    public bool menuUIOpen = false;
    public PlayerShooter playerShooter;
    private float AxisX;
    private float AxisY;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //탄약 업데읻트
    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo.ToString();
        fullAmmoText.text = remainAmmo.ToString();
    }
    //점수 업데이트
    public void SetScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }
    //웨이브 업데이트
    public void SetEnemyText(int count)
    {
        waveText.text = "Enemy Left : " + count;
    }
    //체력 업데이트
    public void SetLifeText(int count)
    {
        lifeText.text = "Life : " + count;
    }
    //크로스해어 업데이트
    public void SetCrossHairPosition(Vector3 worldPosition)
    {
        crosshair.UpdatePosition(worldPosition);
    }
    //최대 체력 슬라이더에 적용
    public void SetHealthMaxSlider(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
    }
    //체력을 보여줌
    public void SetHealthText(float health)
    {
        healthText.text = Mathf.Floor(health).ToString();
        healthSlider.value = health;
    }
    //체력 회복 킷트
    public void SetHealthKitText(int healthKit)
    {
        healthKitText.text = healthKit.ToString();
    }
    //체력 슬라이더의 최대값을 설정함
    public void SetHealthSliderMaxValue(int max)
    {
        restoreHealthSlider.GetComponent<Slider>().maxValue = max;
    }
    //체력 충전 시작
    public void SetActiveRestoreHealthSlider()
    {
        restoreHealthSlider.SetActive(true);
    }
    //체력 충전 업데이트
    public void SetRestoreHealthSlideValuer(int restore)
    {
        restoreHealthSlider.GetComponent<Slider>().value = restore;
    }
    //체력 충전 종료
    public void SetRestoreHealtSliderActiveFalse()
    {
        restoreHealthSlider.SetActive(false);
    }
    //수류탄 개수
    public void SetGrenadeText(int grenade)
    {
        grenadeText.text = grenade.ToString();
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
    public void SetMenuOnOff()
    {
        if (menuUIOpen == false && MenuUI.activeSelf == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            menuUIOpen = true;
            MenuUI.SetActive(true);
            Cursor.visible = true;
            playerShooter.AxisMenuOnChange();
            //Debug.Log("메뉴 오픈");
        }
        else if(menuUIOpen == true && MenuUI.activeSelf == true)
        {
            menuUIOpen = false;
            MenuUI.SetActive(false);
            Cursor.visible = false;
            playerShooter.AxisMenuOffChange();
            SettingByMenu();
            Cursor.lockState = CursorLockMode.Locked;
        }
            
    }
    //인벤토리 오픈
    public void SetInventoryOnOff()
    {
        if (menuUIOpen == false && InventoryUI.activeSelf == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            menuUIOpen = true;
            InventoryUI.SetActive(true);
            Cursor.visible = true;
            playerShooter.AxisMenuOnChange();
            Debug.Log("인벤토리 오픈 오픈");
        }
        else if(menuUIOpen == true && InventoryUI.activeSelf == true)
        {
            menuUIOpen = false;
            InventoryUI.SetActive(false);
            Cursor.visible = false;
            playerShooter.AxisMenuOffChange();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    //장비창 변경
    //배열로 하여 관리하기 ***
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
        if (PlayerInfoManager.Instance.onlineStatus)
        {
            BackEndInventory.InventoryItemList.Clear();// 로그아웃 을했기 때문에 리스트를 초기화 시켜준다.
            BackEndAuthentication.LogOut();// 로그아웃
        }
        SceneManager.LoadScene("Lobby");
    }
    //조준감도 조절 
    public void AxisChangeX()
    {
        AxisX = XAxisSlider.value;
        XAxisText.text = AxisX.ToString();
    }
    
    public void AxisChangeY()
    {
        AxisY = YAxisSlider.value;
        YAxisText.text = AxisY.ToString();
    }
    //메뉴를 닫으면 메뉴에 설정된 값이 적용됨
    public void SettingByMenu()
    {
        playerShooter.AxisChangeX(AxisX);
        playerShooter.AxisChangeY(AxisY);
        PlayerInfoManager.Instance.SetXaxis(AxisX);
        PlayerInfoManager.Instance.SetYaxis(AxisY);
    }
    //게임 시작할때 조준감도 UI 에 적용하여 보여주기
    public void SetAxisUI(float x, float y)
    {
        XAxisSlider.value = x;
        XAxisText.text = x.ToString();
        YAxisSlider.value = y;
        YAxisText.text = y.ToString();
    }

    //아이템 박스 오픈 텍스트 활성화 및 비활성화
    public void OnItemBoxText()
    {
        itemBoxOpenText.SetActive(true);
    }
    public void OffItemBoxText()
    {
        itemBoxOpenText.SetActive(false);
    }
    //폭탄으로 벽 부시기 
    public void OnExplosionWallText()
    {
        explosionWallText.SetActive(true);
    }
    public void OffExplosionWallText()
    {
        explosionWallText.SetActive(false);
    }
    //데미지 텍스트 생성하여 띄우기
    public void OnDamageText(float damage)
    {
        GameObject damageTextObject;
        damageTextObject = DamageTextPooling.Instance.GetObjet(DamageTextParents, damage);
        for (int i = 0; i < DamageTextParents.transform.childCount; i++)
        {
            DamageTextParents.transform.GetChild(i).transform.position = new Vector3(
                DamageTextParents.transform.GetChild(i).transform.position.x,
                DamageTextParents.transform.GetChild(i).transform.position.y + 50f,
                DamageTextParents.transform.GetChild(i).transform.position.z);
        }
        
        StartCoroutine(DamageTextPooling.Instance.ReturnObject(damageTextObject));
    }
}