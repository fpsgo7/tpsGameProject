using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject restoreHealthSlider;
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Text healthText;
    [SerializeField] private Text lifeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text healthKitText;
    [SerializeField] private Text fullAmmoText;
    [SerializeField] private Text waveText;
    
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
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
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
    public void RestoreHealthMax(int max)
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
}