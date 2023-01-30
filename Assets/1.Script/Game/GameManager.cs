using UnityEngine;
/// <summary>
/// 게임의 전체적인 부분을관리하는 매니져이다.
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            
            return instance;
        }
    }

    public Inventory inventory;
    public PlayerShooter playerShooter;

    public int score;
    public int thisGameScore;


    public bool isGameover { get; private set; }
    public bool isOnlineStatus = false;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }
    private void Start()
    {
        //게임메니저에 플레이어 정보를 적용함
        PlayerInfoManager.Instance.SetGameManagerPlayerInfo();
    }

    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;
            thisGameScore += newScore;
            UIManager.Instance.SetScoreText(score);
            PlayerInfoManager.Instance.SavePlayerScore(score);
        }
    }
    
    public void EndGame()
    {
        isGameover = true;
        UIManager.Instance.ActiveGameoverUI(true);
    }

    //게임 시작하면 아이템 장착 
    public void PlayerStartItem(int weaponNum, int equipmentNum)
    {
        inventory.StartAcquireItem();//아이템을 불러온후
        inventory.StartWeaponItem(weaponNum);// 아이템 장착을한다.
        inventory.StartEquipItem(equipmentNum);
    }

    public void PlayerAxisStartSet(float x , float y)
    {
        playerShooter.AxisStartSet(x, y);
    }
}