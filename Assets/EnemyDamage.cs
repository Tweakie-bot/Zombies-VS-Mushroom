using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;

    void Start()
    {

    }
    void Update()
    {

    }
    public int GetDamage()
    {
        return damage;
    }
}
