using UnityEngine;

public class LevelDebug : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;

    [SerializeField]
    private WaveManager waveManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            levelManager.StartLevel();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            levelManager.SkipPreparation();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            waveManager.CompleteCurrentWave();
        }
    }
}