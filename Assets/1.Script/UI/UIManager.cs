using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//스크립트 수정할것!
//tip 이벤트나 다른데서 호출이 되는 함수이름은On 이 좋다.
//2. 무기 창과 장비창 배열로 하여 관리하여 다른 장비창도 들어올 수 있게 설계하기 ***
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
    [SerializeField] private GameObject restoreHealthSliderObject;
    [SerializeField] private GameObject WeaponPanel;
    [SerializeField] private GameObject EquipmentPanel;
    [SerializeField] private GameObject EquipmentChangeButton;
    [SerializeField] private GameObject itemBoxOpenText;
    [SerializeField] private GameObject explosionWallText;
    [SerializeField] private GameObject DamageTextParents;
    [SerializeField] private GameObject damageText;
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider restoreHealthSlider;
    [SerializeField] private Slider xAxisSlider;
    [SerializeField] private Slider yAxisSlider;

    [SerializeField] private Text healthText;
    [SerializeField] private Text lifeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text healthKitText;
    [SerializeField] private Text grenadeText;
    [SerializeField] private Text fullAmmoText;
    [SerializeField] private Text EquipmentChangeButtonText;
    [SerializeField] private Text waveText;
    [SerializeField] private Text xAxisText;
    [SerializeField] private Text yAxisText;



    public bool isMenuUI = false;
    public PlayerShooter playerShooter;
    public PlayerSubCamera playerSubCamera;
    private float xAxis;
    private float yAxis;

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
        restoreHealthSlider.maxValue = max;
    }
    //체력 충전 시작
    public void ActiveRestoreHealthSlider()
    {
        restoreHealthSliderObject.SetActive(true);
    }
    //체력 충전 업데이트
    public void RestoreHealthSlideValue(int restore)
    {
        restoreHealthSlider.value = restore;
    }
    //체력 충전 종료
    public void InactiveRestoreHealtSlider()
    {
        restoreHealthSliderObject.SetActive(false);
    }
    //수류탄 개수
    public void SetGrenadeText(int grenade)
    {
        grenadeText.text = grenade.ToString();
    }
    //크로스헤어 보여주기 관련 
    public void ActiveCrosshair(bool isActive)
    {
        crosshair.SetActiveCrosshair(isActive);
    }
    //게임오버 관련
    public void ActiveGameoverUI(bool isActive)
    {
        gameoverUI.SetActive(isActive);
    }
    //게임 제시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //메뉴 오픈
    public void SetMenuOnOff()
    {
        if (isMenuUI == false && MenuUI.activeSelf == false)
        {
            playerSubCamera.ActiveForrowCamInventory();
            Cursor.lockState = CursorLockMode.Confined;
            isMenuUI = true;
            MenuUI.SetActive(true);
            Cursor.visible = true;
            playerShooter.AxisMenuOnChange();
            //Debug.Log("메뉴 오픈");
        }
        else if(isMenuUI == true && MenuUI.activeSelf == true)
        {
            playerSubCamera.InactiveForrowCamInventory();
            isMenuUI = false;
            MenuUI.SetActive(false);
            Cursor.visible = false;
            //조준감도 설정
            playerShooter.AxisChangeX(xAxis);
            playerShooter.AxisChangeY(yAxis);
            PlayerInfoManager.Instance.SetXaxis(xAxis);
            PlayerInfoManager.Instance.SetYaxis(yAxis);
            playerShooter.AxisMenuOffChange();
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
            isMenuUI = true;
            InventoryUI.SetActive(true);
            Cursor.visible = true;
            playerShooter.AxisMenuOnChange();
            Debug.Log("인벤토리 오픈 오픈");
        }
        else if(isMenuUI == true && InventoryUI.activeSelf == true)
        {
            playerSubCamera.InactiveForrowCamInventory();
            isMenuUI = false;
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
            EquipmentChangeButtonText.text = "장비창으로";
        }
        else
        {
            WeaponPanel.SetActive(false);
            EquipmentPanel.SetActive(true);
            EquipmentChangeButtonText.text = "무기창으로";
        }
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
    //조준감도 조절 
    public void ChangeXAxisSlider()
    {
        xAxis = xAxisSlider.value;
        xAxisText.text = xAxis.ToString();
    }
    
    public void ChangeYAxisSlider()
    {
        yAxis = yAxisSlider.value;
        yAxisText.text = yAxis.ToString();
    }
    //게임 시작할때 조준감도 UI 에 적용하여 보여주기
    public void SetAxisUI(float x, float y)
    {
        xAxisSlider.value = x;
        xAxisText.text = x.ToString();
        yAxisSlider.value = y;
        yAxisText.text = y.ToString();
    }

    //아이템 박스 오픈 텍스트 활성화 및 비활성화
    public void ActiveItemBoxText()
    {
        itemBoxOpenText.SetActive(true);
    }
    public void InactiveItemBoxText()
    {
        itemBoxOpenText.SetActive(false);
    }
    //폭탄으로 벽 부시기 
    public void ActiveExplosionWallText()
    {
        explosionWallText.SetActive(true);
    }
    public void InactiveExplosionWallText()
    {
        explosionWallText.SetActive(false);
    }
    //데미지 텍스트 생성하여 띄우기
    public List<DamageText> damageTexts = new List<DamageText>();
    public void ShowDamageText(float damage,bool isHeadShot)
    {
        DamageText damageText = DamageTextPooling.Instance.GetObject(DamageTextParents, damage,isHeadShot);
        damageTexts.Add(damageText);
        for (int i = 0; i < damageTexts.Count; i++)
        {
            damageTexts[i].transform.position = new Vector3(
                damageTexts[i].transform.position.x,
                damageTexts[i].transform.position.y + 50f,
                damageTexts[i].transform.position.z);
            if (damageTexts[i].isDamageTextActive == false)
            {
                damageTexts.RemoveAt(i);
            }
        }
    }
}