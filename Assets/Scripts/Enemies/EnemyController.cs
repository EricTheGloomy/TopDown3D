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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction += GetHorizontalSeparationDirection() + GetPlayerHorizontalSeparationDirection();
            direction = direction.normalized;

            float minMagnitudeThreshold = 0.01f;
            if (direction.magnitude > minMagnitudeThreshold)
            {
                Vector3 move = direction * moveSpeed * Time.deltaTime;
                characterController.Move(move);

                if (Mathf.Approximately(direction.x, 0) == false || Mathf.Approximately(direction.z, 0) == false)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }

                // Constrain the enemy's position within ground bounds
                ConstrainToBounds();
            }
        }
    }

    private void ConstrainToBounds()
    {
        Vector3 position = transform.position;
        
        // Clamp the enemy's position within the ground bounds
        position.x = Mathf.Clamp(position.x, groundBounds.min.x, groundBounds.max.x);
        position.z = Mathf.Clamp(position.z, groundBounds.min.z, groundBounds.max.z);

        transform.position = position;
    }

    public void SetGroundBounds(Bounds bounds)
    {
        groundBounds = bounds;
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
        float horizontalDistanceToPlayer = new Vector2(toPlayer.x, toPlayer.z).magnitude;

        if (horizontalDistanceToPlayer < playerSeparationDistance)
        {
            Vector3 awayFromPlayer = transform.position - player.position;
            awayFromPlayer.y = 0;
            separationDirection += awayFromPlayer.normalized * separationForce;
        }

        return separationDirection;
    }
}
