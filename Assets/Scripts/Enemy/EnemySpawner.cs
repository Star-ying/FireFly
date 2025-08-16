using System.Collections;
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
    public float healthMultiplier = 1.1f;
    public float speedMultiplier = 1.05f;

     void Start()
    {
        if (SpawnPoints.Length == 0)
        {
            Debug.LogError("生成点或敌人预设未设置！");
            return;
        }
        StartCoroutine(WaveManager());
    }

    private IEnumerator WaveManager()
    {
        CurrentWave++;
        Debug.Log("开始第 " + CurrentWave + " 波攻击！");
        if (CurrentWave - 1 < waves.Length)
        {
            _CurrentWave = waves[CurrentWave - 1];
            _EnemyRemainingAliveCount = _CurrentWave.EnemyCount;
            for (int i = 1; i < _CurrentWave.EnemyCount; i++)
            {
                int spawnindex = Random.Range(0, SpawnPoints.Length);
                _ = Instantiate(_CurrentWave.qiqiPrefab, SpawnPoints[spawnindex].position, Quaternion.identity);
                //QiqiController.OnDeath += onEnemyDeath;
                yield return new WaitForSeconds(_CurrentWave.TimeBetweenSpawn);
            }
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