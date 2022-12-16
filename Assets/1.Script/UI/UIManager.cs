using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//스크립트 수정할것!
//tip 이벤트나 다른데서 호출이 되는 함수이름은On 이 좋다.
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
    [SerializeField] private GameObject gameClearUI;
    [SerializeField] private GameObject itemBoxOpenText;
    [SerializeField] private GameObject explosionWallText;
    [SerializeField] private GameObject DamageTextParents;
    [SerializeField] private GameObject damageText;
    [SerializeField] private Text lifeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;
    [SerializeField] private Text GameClearScoreText;
    public List<DamageText> damageTexts = new List<DamageText>();

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    //생명수 업데이트
    public void SetLifeText(int count)
    {
        lifeText.text = "Life : " + count;
    }
    //점수 업데이트
    public void SetScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }
    //적남은 수 업데이트
    public void SetEnemyText(int count)
    {
        waveText.text = "Enemy Left : " + count;
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
    //게임 클리어
    public void ActiveGameClearUI()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        GameClearScoreText.text = GameManager.Instance.thisGameScore.ToString() + " 점";
        gameClearUI.SetActive(true);
    }
}