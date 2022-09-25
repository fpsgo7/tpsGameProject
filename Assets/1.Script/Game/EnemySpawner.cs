using System.Collections.Generic;
using UnityEngine;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour
{
    private readonly List<Enemy> enemies = new List<Enemy>();//적생성기

    public float damageMax = 40f;//생성할 적의 대미지
    public float damageMin = 20f;
    public Enemy enemyPrefab;//적 프리팹
    //생성할 적의 최대 최소 범위
    public float healthMax = 200f;
    public float healthMin = 100f;

    public Transform[] spawnPoints;//배열변수
    //생성할 적의 속도 최대 최소 범위
    public float speedMax = 12f;
    public float speedMin = 3f;
    //좀비 생성 결정
    public Color strongEnemyColor = Color.red;
    private int wave;//웨이브

    private void Update()
    {
        //게임오버가 되면 진행을 막느낟.
        if (GameManager.Instance != null && GameManager.Instance.isGameover) return;
        //적이 다죽으면 적 스폰을 실행해준다.
        if (enemies.Count <= 0) SpawnWave();
        //유아이 업데이트
        UpdateUI();
    }

    private void UpdateUI()
    {
        //유아이 메니져에 현제 웨이브와 적 수를 보여준다.
        UIManager.Instance.UpdateWaveText(wave, enemies.Count);
    }
    //적 스폰에서 웨이브와 적 새
    private void SpawnWave()
    {
        wave++;


        var spawnCount = Mathf.RoundToInt(wave * 5); //현제 웨이브에 곱하기 5한만큼 적생성

        for (var i = 0; i < spawnCount; i++)
        {
            var enemyIntensity = Random.Range(0f, 1f);

            CreateEnemy(enemyIntensity);
        }
    }

    private void CreateEnemy(float intensity)
    {
        var health = Mathf.Lerp(healthMin, healthMax, intensity);
        var damage = Mathf.Lerp(damageMin, damageMax, intensity);
        var speed = Mathf.Lerp(speedMin, speedMax, intensity);

        var skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        //enemy.Setup(health, damage, speed, speed * 0.3f, skinColor);

        enemies.Add(enemy);

        enemy.OnDeath += () => enemies.Remove(enemy);// 사망한 대상은 리스트에서 제외한다.
        enemy.OnDeath += () => Destroy(enemy.gameObject, 10f);
        enemy.OnDeath += () => GameManager.Instance.AddScore(100);

    }
}