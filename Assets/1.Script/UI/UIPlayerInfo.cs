using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 플레이어 정보를 나타내는 UI 스크립트
/// </summary>

public class UIPlayerInfo : MonoBehaviour
{
    //싱글톤 방식 사용
    private static UIPlayerInfo instance;

    public static UIPlayerInfo Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIPlayerInfo>();

            return instance;
        }
    }

    //각 오브젝트들을 유니티 인스팩터 상에서 연결하기위하여 SerializeField 사용
    [SerializeField] private GameObject restoreHealthSliderObject;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider restoreHealthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text healthKitText;
    [SerializeField] private Text grenadeText;
    [SerializeField] private Text fullAmmoText;

    //탄약 업데읻트
    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo.ToString();
        fullAmmoText.text = remainAmmo.ToString();
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
}
