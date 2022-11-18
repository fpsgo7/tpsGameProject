using UnityEngine;

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

    public GameObject firstMap;
    public GameObject firstDoor;
    public GameObject SecondMap;
    public Inventory inventory;
    public PlayerShooter playerShooter;

    public int score;
    private int enemyCount;

    public bool isGameover { get; private set; }

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }
    private void Start()
    {
        JsonPlayerInfoManager.Instance.LoadPlayer();
    }
    
    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;
            UIManager.Instance.UpdateScoreText(score);
            JsonPlayerInfoManager.Instance.SavePlayerScore(score);
        }
    }
    
    public void EndGame()
    {
        isGameover = true;
        UIManager.Instance.SetActiveGameoverUI(true);
    }
    //애너미 생성과 죽음 수 관리 
    public void EnemyMake()
    {
        enemyCount += 1;
        UIManager.Instance.UpdateEnemyText(enemyCount);
    }

    public void EnemyDie()
    {
        enemyCount -= 1;
        UIManager.Instance.UpdateEnemyText(enemyCount);
        if(enemyCount == 0 && firstMap.activeSelf== true)
        {
            firstDoor.GetComponent<doorOpen>().doorRock = false;
        }
    }
    //다음맵 이동
    public void NextMap()
    {
        if (firstMap.activeSelf == true)
            SecondMap.SetActive(true);
    }

    //게임 시작하면 아이템 장착 
    public void PlayerStartItem(string weapon, string equipment)
    {
        inventory.StartAcquireItem();//아이템을 불러온후
        inventory.StartEquipItem(weapon, equipment);// 아이템 장착을한다.
    }

    public void PlayerAxisStartSet(float x , float y)
    {
        playerShooter.AxisStartSet(x, y);
    }
}