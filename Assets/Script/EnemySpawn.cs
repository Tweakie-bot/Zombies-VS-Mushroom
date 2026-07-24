using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private EnemyPath path;

    public EnemyPath GetEnemyPath()
    {
        return path;
    }
}