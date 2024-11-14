// Scripts/Weapons/WeaponController.cs
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponStats weaponStats;
    public GameObject projectilePrefab;

    private float currentDamage; // Local instance-based damage
    private float fireCooldown = 0f;

    private void Start()
    {
        // Initialize instance damage from WeaponStats
        currentDamage = weaponStats.damage;
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Transform nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                Fire(nearestEnemy.position);
                fireCooldown = 1f / weaponStats.fireRate;
            }
        }
    }

    private void Fire(Vector3 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        // Pass instance-specific damage to projectile
        projectileController.Initialize(
            targetPosition,
            currentDamage, // Use instance-based damage
            weaponStats.projectileSpeed,
            weaponStats.range
        );
    }

    public void ApplyDamageUpgrade(float multiplier)
    {
        currentDamage *= multiplier; // Only affects this instance
        Debug.Log("New Weapon Damage: " + currentDamage);
    }

    private Transform FindNearestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }
}
