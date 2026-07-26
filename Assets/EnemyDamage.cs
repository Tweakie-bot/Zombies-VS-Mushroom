using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;

    public int Damage => damage;
}