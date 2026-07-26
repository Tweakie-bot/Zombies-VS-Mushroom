using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 30;

    private int currentHealth;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log($"{gameObject.name} HP: {currentHealth}");

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        EnemyWaveMember waveMember =
            GetComponent<EnemyWaveMember>();

        if (waveMember != null)
        {
            waveMember.Die();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}