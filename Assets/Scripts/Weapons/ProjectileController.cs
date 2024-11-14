// Scripts/Weapons/ProjectileController.cs
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float damage;
    private float speed;
    private float maxRange;
    
    private Vector3 direction;
    private Vector3 startPosition;
    private Rigidbody rb;

    public void Initialize(Vector3 targetPosition, float damage, float speed, float maxRange)
    {
        // Set up initial properties from WeaponStats
        this.damage = damage;
        this.speed = speed;
        this.maxRange = maxRange;
        
        startPosition = transform.position;

        // Calculate and store the initial direction toward the target position
        direction = (targetPosition - startPosition).normalized;

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Ensure Rigidbody settings for precise control
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void Update()
    {
        // Move the projectile in the fixed direction with Rigidbody
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

        // Check if the projectile has exceeded its max range
        if (Vector3.Distance(startPosition, transform.position) >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the projectile collided with an enemy
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // Apply damage to the enemy
            }

            Destroy(gameObject); // Destroy the projectile upon impact with the enemy
        }
        // Check if the projectile collided with an obstacle
        else if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject); // Destroy the projectile upon impact with an obstacle
        }
    }
}
