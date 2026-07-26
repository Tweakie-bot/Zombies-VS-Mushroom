using UnityEngine;

public class HeroHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;

    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log($"Hero HP : {currentHealth}");

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Hero died");

        LevelManager levelManager = FindAnyObjectByType<LevelManager>();

        if (levelManager != null)
        {
            levelManager.GameOver();
        }
    }
}