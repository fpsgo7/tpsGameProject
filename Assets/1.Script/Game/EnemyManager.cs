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
    private const string firstMap = "FirstMap";
    private const string bridgeLeftMap = "BridgeLeftMap";
    private const string bridgeRightMap = "BridgeRightMap";
    private const string lastMap = "LastMap";
    private string mapName;
    private bool bridgeLeftKey;//아이템을 얻기 위한 통곡의 다리
    private bool bridgeRightKey;// 다음 단계를 가기위한 통곡의 다리
    private bool lastMaplKey;
    private bool mapIn;// 해당 던전안에 있는지 체크

    private int enemyCount;


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
        if (enemyCount == 0)
        {
            MapClear();
        }
    }

    public void StartEnemySpawnerFirstMap(string mapName)
    {
        if(mapIn == false)
        {
            enemySpawnerFirstMap.SetActive(true);
            mapIn = true;
            this.mapName = mapName;
        }
    }

    public void StartEnemySpawnerBridgeLeftMap(string mapName)
    {
        if (mapIn == false && bridgeLeftKey == true)
        {
            enemySpawnerBridgeLeftMap.SetActive(true);
            mapIn = true;
            this.mapName = mapName;
        }
    }

    public void StartEnemySpawnerBridgeRightMap(string mapName)
    {
        if (mapIn == false && bridgeRightKey == true)
        {
            enemySpawnerBridgeRightMap.SetActive(true);
            mapIn = true;
            this.mapName = mapName;
        }
    }

    public void StartEnemySpawnerLastMap(string mapName)
    {
        if (mapIn == false && lastMaplKey == true)
        {
            enemySpawnerLastMap.SetActive(true);
            mapIn = true;
            this.mapName = mapName;
        }
    }

    public void MapClear()
    {
        mapIn = false;
        if (mapName == firstMap)
        {
            bridgeLeftKey = true;
            bridgeRightKey = true;
        }
        else if (mapName == bridgeLeftMap) 
        {
            //아이템 박스 활성화
        }
        else if( mapName == bridgeRightMap)
        {
            lastMaplKey = true;
        }
        else if(mapName == lastMap)
        {
            // 게임클리어 관련 내용 실행
        }
    }
}
