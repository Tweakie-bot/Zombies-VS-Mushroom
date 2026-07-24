using UnityEngine;

public class EnemyWaveMember : MonoBehaviour
{
    public WaveManager waveManager { get; private set; }

    public void Initialize(WaveManager manager)
    {
        waveManager = manager;
    }
}