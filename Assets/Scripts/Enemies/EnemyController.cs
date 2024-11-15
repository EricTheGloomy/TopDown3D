// Scripts/Enemies/EnemyController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float separationDistance = 1f;
    public float playerSeparationDistance = 1f;
    public float separationForce = 1f;

    private Transform player;
    private CharacterController characterController;
    private Bounds groundBounds;

    private bool isInitialized = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Attempt to find player
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError($"{name}: Player reference not found! Ensure the player has the correct tag.");
        }

        // Check if initialization is ready
        if (groundBounds != null && player != null)
        {
            isInitialized = true;
        }
        else
        {
            Debug.LogWarning($"{name}: Enemy not fully initialized. Waiting for dependencies...");
        }
    }

    private void Update()
    {
        if (!isInitialized)
        {
            Debug.LogWarning($"{name}: Skipping Update, not initialized.");
            return; // Wait until the enemy is fully initialized
        }

        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction += GetHorizontalSeparationDirection() + GetPlayerHorizontalSeparationDirection();
            direction = direction.normalized;

            float minMagnitudeThreshold = 0.01f; // Prevent zero vector warnings
            if (direction.magnitude > minMagnitudeThreshold)
            {
                Vector3 move = direction * moveSpeed * Time.deltaTime;
                characterController.Move(move);

                // Only rotate if there is meaningful movement
                if (!Mathf.Approximately(direction.x, 0) || !Mathf.Approximately(direction.z, 0))
                {
                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }

                // Constrain the enemy's position within ground bounds
                ConstrainToBounds();
            }
            else
            {
                Debug.Log($"{name}: No valid movement direction. Possibly stuck or too close to obstacles.");
            }
        }
    }

    private void ConstrainToBounds()
    {
        Vector3 position = transform.position;

        // Adjust bounds by CharacterController extents
        if (characterController != null)
        {
            float radius = characterController.radius;
            position.x = Mathf.Clamp(position.x, groundBounds.min.x + radius, groundBounds.max.x - radius);
            position.z = Mathf.Clamp(position.z, groundBounds.min.z + radius, groundBounds.max.z - radius);
        }

        transform.position = position;
    }

    public void SetGroundBounds(Bounds bounds)
    {
        groundBounds = bounds;

        // Re-check initialization state after setting ground bounds
        if (groundBounds != null && player != null)
        {
            isInitialized = true;
            Debug.Log($"{name}: Ground bounds set and enemy initialized.");
        }
    }

    private Vector3 GetHorizontalSeparationDirection()
    {
        Vector3 separationDirection = Vector3.zero;
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, separationDistance);

        foreach (Collider enemyCollider in nearbyEnemies)
        {
            if (enemyCollider.gameObject != gameObject && enemyCollider.CompareTag("Enemy"))
            {
                Vector3 awayFromEnemy = transform.position - enemyCollider.transform.position;
                awayFromEnemy.y = 0;
                separationDirection += awayFromEnemy.normalized * separationForce;
            }
        }

        return separationDirection;
    }

    private Vector3 GetPlayerHorizontalSeparationDirection()
    {
        Vector3 separationDirection = Vector3.zero;
        Vector3 toPlayer = player.position - transform.position;

        if (toPlayer.magnitude < playerSeparationDistance)
        {
            Vector3 awayFromPlayer = transform.position - player.position;
            awayFromPlayer.y = 0;
            separationDirection += awayFromPlayer.normalized * separationForce;
        }

        return separationDirection;
    }
}
