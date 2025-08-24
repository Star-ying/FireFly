using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] SpawnPoints;

    [Header("波次设置")]
    public Wave[] waves;
    private Wave _CurrentWave;
    private int CurrentWave;

    private int _EnemyRemainingAliveCount;

    [Header("难度系数")]
    public float healthMultiplierInspector = 1.1f;
    public float speedMultiplierInspector = 1.05f;
    public static float healthMultiplier;
    public static float speedMultiplier;

    private void Start()
    {
        healthMultiplier = healthMultiplierInspector;
        speedMultiplier = speedMultiplierInspector;
        
        if (SpawnPoints.Length != 0&&GameManager.Exists )
        {
            GameManager.Instance.GameStart.AddListener(StartSpawning);
            GameManager.Instance.GameOver.AddListener(StopSpawning);
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(WaveManager());
    }
    public void StopSpawning()
    {
        StopAllCoroutines();
    }
    private IEnumerator WaveManager()
    {
        CurrentWave++;
        //Debug.Log("开始第 " + CurrentWave + " 波攻击！");
        if (CurrentWave - 1 < waves.Length)
        {
            _CurrentWave = waves[CurrentWave - 1];
            _EnemyRemainingAliveCount = _CurrentWave.EnemyCount;
            for (int i = 1; i < _CurrentWave.EnemyCount; i++)
            {
                int spawnindex = Random.Range(0, SpawnPoints.Length);
                QiqiController qiqiController= Instantiate(_CurrentWave.qiqiPrefab, SpawnPoints[spawnindex].position, Quaternion.identity);
                qiqiController.OnDeath += onEnemyDeath;
                yield return new WaitForSeconds(_CurrentWave.TimeBetweenSpawn);
            }
        }
        else
        {
            CurrentWave = 0;
            StartCoroutine(WaveManager());
        }
    }
    private void onEnemyDeath()
    {
        _EnemyRemainingAliveCount--;
        if (_EnemyRemainingAliveCount == 0)
        {
            StartCoroutine(WaveManager());
        }
    }
}