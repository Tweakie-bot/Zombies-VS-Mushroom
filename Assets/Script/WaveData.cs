using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
    [SerializeField]
    private List<EnemySpawnEntry> enemySpawns = new();
    public IReadOnlyList<EnemySpawnEntry> EnemySpawns => enemySpawns;
}