using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (other.tag=="Player" )
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
