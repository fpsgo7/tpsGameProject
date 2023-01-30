using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어가 특정 구역에 도착하면 적들을 생성하는 
/// 클래스를 시작하게 하는 클래스이다.
/// </summary>
public class EnemySpawnStart : MonoBehaviour
{
    public enum Maps
    {
        None,
        FirstMap,
        BridgeLeftMap,
        BridgeRightMap,
        LastMap
    }
    public Maps maps = Maps.None;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(maps == Maps.FirstMap)
                EnemyManager.Instance.StartEnemySpawnerFirstMap(maps.ToString());
            if(maps == Maps.BridgeLeftMap)
                EnemyManager.Instance.StartEnemySpawnerBridgeLeftMap(maps.ToString());
            if(maps == Maps.BridgeRightMap)
                EnemyManager.Instance.StartEnemySpawnerBridgeRightMap(maps.ToString());
            if(maps == Maps.LastMap)
                EnemyManager.Instance.StartEnemySpawnerLastMap(maps.ToString());
        }
    }
}
