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

    [SerializeField]
    private HeroHealth heroHealth;

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

        CheckWaveCompletion();
    }

    private void RemoveEnemy(EnemyWaveMember enemy)
    {
        if (enemy == null)
        {
            return;
        }

        activeEnemies.Remove(enemy);

        Destroy(enemy.gameObject);

        CheckWaveCompletion();
    }

    public void EnemyReachedHero(EnemyWaveMember enemy)
    {
        if (enemy == null)
        {
            return;
        }

        EnemyDamage enemyDamage =
            enemy.GetComponent<EnemyDamage>();

        if (heroHealth != null && enemyDamage != null)
        {
            heroHealth.TakeDamage(enemyDamage.Damage);
        }

        RemoveEnemy(enemy);
    }

    public void EnemyDied(EnemyWaveMember enemy)
    {
        RemoveEnemy(enemy);
    }

    private void CheckWaveCompletion()
    {
        if (!isWaveRunning)
        {
            return;
        }

        IReadOnlyList<EnemySpawnEntry> spawnEntries =
            waves[currentWaveIndex].EnemySpawns;

        bool allEnemiesSpawned =
            nextSpawnIndex >= spawnEntries.Count;

        bool noEnemiesRemaining =
            activeEnemies.Count == 0;

        if (allEnemiesSpawned && noEnemiesRemaining)
        {
            CompleteCurrentWave();
        }
    }
    public void StopWave()
    {
        if (!isWaveRunning)
        {
            return;
        }

        isWaveRunning = false;

        DestroyRemainingEnemies();
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

        Debug.Log($"Wave {currentWaveIndex + 1} started.");
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

        if (currentWaveIndex >= waves.Count)
        {
            levelManager.CompleteLevel();
        }
        else
        {
            levelManager.CompleteWave();
        }
    }

    private void DestroyRemainingEnemies()
    {
        foreach (EnemyWaveMember enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        activeEnemies.Clear();
    }
}