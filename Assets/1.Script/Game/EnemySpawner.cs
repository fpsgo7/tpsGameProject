﻿using System.Collections.Generic;
using UnityEngine;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour
{
    private readonly List<GameObject> enemies = new List<GameObject>();//적생성기

   
    public GameObject enemyPrefab;//적 프리팹
    private PlayerRader playerRader;
    //생성할 적의 최대 최소 범위
    public float healthMax = 200f;
    public float healthMin = 100f;
    public float damageMax = 40f;//생성할 적의 대미지
    public float damageMin = 20f;
    public Transform[] spawnPoints;//배열변수
    //생성할 적의 속도 최대 최소 범위
    public float speedMax = 12f;
    public float speedMin = 3f;
    //좀비 생성 결정
    public Color strongEnemyColor = Color.red;
    private int wave;//웨이브
    public int EnemyCount;

    private void Awake()
    {
        playerRader = GameObject.Find("Player").GetComponent<PlayerRader>();
    }
    private void Start()
    {
        Spawn();
    }
   
    //적 스폰에서 웨이브와 적 새
    private void Spawn()
    {
        //적들을 생성합니다.
        for (var i = 0; i < EnemyCount; i++)
        {
            var enemyIntensity = Random.Range(0, 3);
            EnemyManager.Instance.EnemyMake();
            CreateEnemy(enemyIntensity);
        }
    }

    private void CreateEnemy(int intensity)
    {
        //Debug.Log(intensity);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.GetComponent<LivingEntity>().EnemySetup(intensity);

        enemies.Add(enemy);
        playerRader.GetTrackedObjects(enemy);

        enemy.GetComponent<LivingEntity>().OnDeath += () => playerRader.RemoveTrackedObject(enemy);
        enemy.GetComponent<LivingEntity>().OnDeath += () => enemies.Remove(enemy);// 사망한 대상은 리스트에서 제외한다.
        enemy.GetComponent<LivingEntity>().OnDeath += () => EnemyManager.Instance.EnemyDie();
        enemy.GetComponent<LivingEntity>().OnDeath += () => Destroy(enemy.gameObject, 3f);
        enemy.GetComponent<LivingEntity>().OnDeath += () => enemy.GetComponent<ItemSpawn>().Spawn();
        enemy.GetComponent<LivingEntity>().OnDeath += () => GameManager.Instance.AddScore(100);

    }
}