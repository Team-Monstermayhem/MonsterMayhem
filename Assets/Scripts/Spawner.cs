using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;
    float prefabChangeTimer;  
    List<int> enemyPrefabIndexes;  
    int currentPrefabIndex;  
    int currentWave = 1;      
    float waveDuration = 5f;  
    int maxWave = 10;         

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();

        enemyPrefabIndexes = new List<int>();
        for (int i = 0; i < GameManager.instance.poolManager.prefabs.Length; i++)
        {
            if (GameManager.instance.poolManager.prefabs[i].CompareTag("Enemy")) 
            {
                enemyPrefabIndexes.Add(i);
            }
        }
    }

    private void Start()
    {
        currentPrefabIndex = enemyPrefabIndexes[0];  

        // 웨이브 데이터 자동 생성
        spawnData = new SpawnData[maxWave];

        for (int i = 0; i < maxWave; i++)
        {
            float spawnTime = Mathf.Max(0.1f, 1.0f - (i * 0.05f));  // 스폰 시간은 점진적으로 감소하며, 최소 0.1초
            int health = 10 + (i * 10);  // 웨이브마다 적 체력 10씩 증가
            float speed = 1.0f + (i * 0.1f);  // 웨이브마다 적 속도 0.1씩 증가
            spawnData[i] = new SpawnData { spawnTime = spawnTime, spriteType = i % enemyPrefabIndexes.Count, health = health, speed = speed };
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        prefabChangeTimer += Time.deltaTime;

        // 웨이브 변경 시간 체크
        if (prefabChangeTimer > waveDuration)
        {
            prefabChangeTimer = 0f;
            NextWave();  
        }

        // 현재 레벨 계산 (게임 시간이 10초마다 한 레벨 증가)
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        // 적 스폰 타이밍 체크
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();  
        }
    }

    void NextWave()
    {
        if (currentWave < maxWave)
        {
            currentWave++;
            int nextIndex = (enemyPrefabIndexes.IndexOf(currentPrefabIndex) + 1) % enemyPrefabIndexes.Count;
            currentPrefabIndex = enemyPrefabIndexes[nextIndex];
            Debug.Log("웨이브 " + currentWave + " 시작!");
        }
        else
        {
            Debug.Log("모든 웨이브 완료!");
        }
    }

    void Spawn()
    {
        // 현재 웨이브에 따라 스폰할 적의 수 계산
        int enemiesToSpawn = currentWave + 1;  // 웨이브 수에 따라 스폰할 적의 수 증가

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemy = GameManager.instance.poolManager.GetObject(currentPrefabIndex);
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.Init(spawnData[level]);

            // 웨이브에 따른 적의 체력과 속도 설정
            enemyComponent.health = spawnData[level].health + (currentWave * 1.5f);
            enemyComponent.speed = spawnData[level].speed - (currentWave * 0.1f);
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
