// Scripts/Player/PlayerHealth.cs
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerStats playerStats;
    private float currentHealth;

    private void Start()
    {
        currentHealth = playerStats.health; // Initialize from PlayerStats
    }

    public void ApplyHealthUpgrade(float extraHealth)
    {
        currentHealth += extraHealth;
        if (currentHealth > playerStats.health)
        {
            currentHealth = playerStats.health;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
    }
}
