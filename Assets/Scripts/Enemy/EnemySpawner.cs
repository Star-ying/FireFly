using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("敌人预设")]
    public GameObject qiqiPrefab; 

    [Header("生成设置")]
    public Transform[] spawnPoints; 
    public float initialSpawnRate = 2f; 
    public float spawnRateDecrease = 0.1f; 
    public int initialEnemiesPerWave = 5; 
    public int enemiesIncreasePerWave = 2; 

    [Header("波次设置")]
    public float timeBetweenWaves = 5f; 

    private int currentWave = 0; 
    private float currentSpawnRate; 
    private int enemiesToSpawn; 
    private bool isSpawning = false; 


    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        StartCoroutine(WaveManager());
    }

    
    IEnumerator WaveManager()
    {
        while (true) 
        {
           
            yield return new WaitForSeconds(timeBetweenWaves);

            
            currentWave++;
            Debug.Log("开始第 " + currentWave + " 波攻击！");

            
            enemiesToSpawn = initialEnemiesPerWave + (currentWave - 1) * enemiesIncreasePerWave;

            
            isSpawning = true;
            StartCoroutine(SpawnEnemies());

            
            yield return new WaitWhile(() => isSpawning);

            
            currentSpawnRate = Mathf.Max(0.5f, initialSpawnRate - (currentWave * spawnRateDecrease));
        }
    }

   
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnSingleQiqi();
            yield return new WaitForSeconds(currentSpawnRate);
        }

        isSpawning = false;
    }

    
    void SpawnSingleQiqi()
    {
        if (spawnPoints.Length == 0 || qiqiPrefab == null)
        {
            Debug.LogError("生成点或敌人预设未设置！");
            return;
        }

        
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];




        QiqiController qiqiController = Instantiate(qiqiPrefab, spawnPoint.position, Quaternion.identity).GetComponent<QiqiController>();
        if (qiqiController != null)
        {
            qiqiController.SetDifficulty(currentWave);
        }
    }
}