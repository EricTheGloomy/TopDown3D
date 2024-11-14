// Scripts/Enemies/EnemyHealth.cs
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 50f;
    public GameObject experiencePickupPrefab; // Reference to the ExperiencePickup prefab

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Spawn an experience pickup at the enemy's position
        if (experiencePickupPrefab != null)
        {
            Instantiate(experiencePickupPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Destroy the enemy
    }
}
