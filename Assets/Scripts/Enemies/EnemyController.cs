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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (player != null)
        {
            // Calculate direction towards the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Apply separation forces
            direction += GetHorizontalSeparationDirection() + GetPlayerHorizontalSeparationDirection();

            // Normalize the final direction vector
            direction = direction.normalized;

            // Minimum magnitude threshold to ensure direction is meaningful
            float minMagnitudeThreshold = 0.01f;

            if (direction.magnitude > minMagnitudeThreshold)
            {
                // Move towards the player while maintaining separation
                Vector3 move = direction * moveSpeed * Time.deltaTime;
                characterController.Move(move);

                // Rotate to face the player only if direction.x or direction.z is significant
                if (Mathf.Approximately(direction.x, 0) == false || Mathf.Approximately(direction.z, 0) == false)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }
            }
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
                awayFromEnemy.y = 0; // Ignore vertical component
                separationDirection += awayFromEnemy.normalized * separationForce;
            }
        }

        return separationDirection;
    }

    private Vector3 GetPlayerHorizontalSeparationDirection()
    {
        Vector3 separationDirection = Vector3.zero;

        Vector3 toPlayer = player.position - transform.position;
        float horizontalDistanceToPlayer = new Vector2(toPlayer.x, toPlayer.z).magnitude;

        if (horizontalDistanceToPlayer < playerSeparationDistance)
        {
            Vector3 awayFromPlayer = transform.position - player.position;
            awayFromPlayer.y = 0; // Ignore vertical component
            separationDirection += awayFromPlayer.normalized * separationForce;
        }

        return separationDirection;
    }
}
