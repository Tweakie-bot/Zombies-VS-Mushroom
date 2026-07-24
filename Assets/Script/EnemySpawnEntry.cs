using System;
using UnityEngine;

[Serializable]
public class EnemySpawnEntry
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float spawnTime;

    [SerializeField]
    private EnemySpawn spawnPoint;

    public GameObject EnemyPrefab => enemyPrefab;
    public float SpawnTime => spawnTime;
    public EnemySpawn SpawnPoint => spawnPoint;
}