using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;

    public static EnemyManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<EnemyManager>();

            return instance;
        }
    }
   

    public GameObject enemySpawnerFirstMap;
    public GameObject enemySpawnerBridgeLeftMap;
    public GameObject enemySpawnerBridgeRightMap;
    public GameObject enemySpawnerLastMap;
    public GameObject RightFence1;
    public GameObject RightFence2;
    public GameObject RightEndFence1;
    public GameObject RightEndFence2;
    public GameObject LeftFence1;
    public GameObject LeftFence2;
    private Animator rightFence1Animator;
    private Animator rightFence2Animator;
    private Animator leftFence1Animator;
    private Animator leftFence2Animator;
    private Animator rightEndFence1Animator;
    private Animator rightEndFence2Animator;
    private readonly int fenceLeftMove = Animator.StringToHash("LeftOpen");
    private readonly int fenceRightMove = Animator.StringToHash("RightOpen");
    private const string firstMap = "FirstMap";
    private const string bridgeLeftMap = "BridgeLeftMap";
    private const string bridgeRightMap = "BridgeRightMap";
    private const string lastMap = "LastMap";
    private string mapName;
    public bool isBridgeLeftKey;//아이템을 얻기 위한 통곡의 다리
    public bool isBridgeRightKey;// 다음 단계를 가기위한 통곡의 다리
    public bool isLastMaplKey;
    private bool isMapIn;// 해당 던전안에 있는지 체크
    private int enemyCount;

    private void Awake()
    {
        rightFence1Animator = RightFence1.GetComponent<Animator>();
        rightFence2Animator = RightFence2.GetComponent<Animator>();
        leftFence1Animator = LeftFence1.GetComponent<Animator>();
        leftFence2Animator = LeftFence2.GetComponent<Animator>();
        rightEndFence1Animator = RightEndFence1.GetComponent<Animator>();
        rightEndFence2Animator = RightEndFence2.GetComponent<Animator>();

    }
    //애너미 생성과 죽음 수 관리 
    public void EnemyMake()
    {
        enemyCount += 1;
        UIManager.Instance.SetEnemyText(enemyCount);
    }

    public void EnemyDie()
    {
        enemyCount -= 1;
        UIManager.Instance.SetEnemyText(enemyCount);
        if (enemyCount == 0)
        {
            MapClear();
        }
    }

    public void StartEnemySpawnerFirstMap(string mapName)
    {
        if(isMapIn == false)
        {
            enemySpawnerFirstMap.SetActive(true);
            isMapIn = true;
            this.mapName = mapName;
        }
    }

    public void StartEnemySpawnerBridgeLeftMap(string mapName)
    {
        if (isMapIn == false && isBridgeLeftKey == true)
        {
            leftFence1Animator.SetTrigger(fenceLeftMove);
            leftFence2Animator.SetTrigger(fenceRightMove);
            enemySpawnerBridgeLeftMap.SetActive(true);
            isMapIn = true;
            this.mapName = mapName;
        }
    }

    public void StartEnemySpawnerBridgeRightMap(string mapName)
    {
        if (isMapIn == false && isBridgeRightKey == true)
        {
            rightFence1Animator.SetTrigger(fenceLeftMove);
            rightFence2Animator.SetTrigger(fenceRightMove);
            enemySpawnerBridgeRightMap.SetActive(true);
            isMapIn = true;
            this.mapName = mapName;
        }
    }

    public void StartEnemySpawnerLastMap(string mapName)
    {
        if (isMapIn == false && isLastMaplKey == true)
        {
            enemySpawnerLastMap.SetActive(true);
            isMapIn = true;
            this.mapName = mapName;
        }
    }

    public void MapClear()
    {
        isMapIn = false;
        if (mapName.Equals(firstMap))
        {
            isBridgeLeftKey = true;
            isBridgeRightKey = true;
        }
        if (mapName.Equals(bridgeLeftMap)) 
        {
            //아이템 박스 활성화
        }
        if( mapName.Equals(bridgeRightMap))
        {
            isLastMaplKey = true;
            rightEndFence1Animator.SetTrigger(fenceLeftMove);
            rightEndFence2Animator.SetTrigger(fenceRightMove);
        }
        if(mapName.Equals(lastMap))
        {
            // 게임클리어 관련 내용 실행
            UIManager.Instance.ActiveGameClearUI();
        }
    }
}
