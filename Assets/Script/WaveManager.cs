using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private LevelManager levelManager;

    [Header("Waves")]
    [SerializeField]
    private List<WaveData> waves = new();

    private readonly List<EnemyWaveMember> activeEnemies = new();

    private int currentWaveIndex;
    private int nextSpawnIndex;

    private float waveElapsedTime;
    private bool isWaveRunning;

    private void Update()
    {
        if (!isWaveRunning)
        {
            return;
        }

        waveElapsedTime += Time.deltaTime;

        SpawnEnemiesReadyToAppear();
    }

    private void SpawnEnemiesReadyToAppear()
    {
        IReadOnlyList<EnemySpawnEntry> spawnEntries = waves[currentWaveIndex].EnemySpawns;

        while (nextSpawnIndex < spawnEntries.Count && spawnEntries[nextSpawnIndex].SpawnTime <= waveElapsedTime)
        {
            SpawnEnemy(spawnEntries[nextSpawnIndex]);
            nextSpawnIndex++;
        }
    }

    private void SpawnEnemy(EnemySpawnEntry spawnEntry)
    {
        if (spawnEntry.EnemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is missing.");
            return;
        }

        if (spawnEntry.SpawnPoint == null)
        {
            Debug.LogWarning("Enemy spawn point is missing.");
            return;
        }

        EnemyPath path = spawnEntry.SpawnPoint.GetEnemyPath();

        if (path == null)
        {
            Debug.LogWarning($"{spawnEntry.SpawnPoint.name} has no assigned path.");

            return;
        }

        GameObject enemyObject = Instantiate(spawnEntry.EnemyPrefab, spawnEntry.SpawnPoint.transform.position, spawnEntry.SpawnPoint.transform.rotation );

        EnemyWaveMember waveMember = enemyObject.GetComponent<EnemyWaveMember>();

        EnemyMovement movement = enemyObject.GetComponent<EnemyMovement>();

        if (waveMember == null || movement == null)
        {
            Debug.LogWarning( $"{enemyObject.name} needs EnemyWaveMember and EnemyMovement." );

            Destroy(enemyObject);
            return;
        }

        waveMember.Initialize(this);
        movement.OnInitialize(path);

        activeEnemies.Add(waveMember);
    }

    public void StartWave()
    {
        if (isWaveRunning)
        {
            return;
        }

        if (currentWaveIndex >= waves.Count)
        {
            return;
        }

        waveElapsedTime = 0;
        nextSpawnIndex = 0;
        isWaveRunning = true;

        activeEnemies.Clear();
    }

    public void CompleteCurrentWave()
    {
        if (!isWaveRunning)
        {
            return;
        }

        isWaveRunning = false;

        DestroyRemainingEnemies();

        currentWaveIndex++;

        Debug.Log("Wave completed.");

        levelManager.CompleteWave();
    }

    private void DestroyRemainingEnemies()
    {
        foreach (EnemyWaveMember enemy in activeEnemies)
        {
            Destroy(enemy.gameObject);
        }

        activeEnemies.Clear();
    }
}