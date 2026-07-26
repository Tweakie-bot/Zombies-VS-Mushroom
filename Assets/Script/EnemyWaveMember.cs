using UnityEngine;

public class EnemyWaveMember : MonoBehaviour
{
    public WaveManager WaveManager { get; private set; }

    public void Initialize(WaveManager manager)
    {
        WaveManager = manager;
    }

    public void ReachEndOfPath()
    {
        if (WaveManager == null)
        {
            return;
        }

        WaveManager.EnemyReachedHero(this);
    }

    public void Die()
    {
        if (WaveManager == null)
        {
            Destroy(gameObject);
            return;
        }

        WaveManager.EnemyDied(this);
    }
}