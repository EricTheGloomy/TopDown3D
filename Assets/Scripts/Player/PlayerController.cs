// Scripts/Player/PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;
    private CharacterController characterController;
    private float moveSpeed;
    private Bounds groundBounds; // Dynamically set ground bounds

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        moveSpeed = playerStats.moveSpeed;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection != Vector3.zero)
        {
            Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
            characterController.Move(move);

            transform.forward = moveDirection;

            // Constrain the player's position within ground bounds
            ConstrainToBounds();
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

        transform.position = position; // Apply clamped position
    }

    public void SetGroundBounds(Bounds bounds)
    {
        groundBounds = bounds; // Set the ground bounds dynamically
    }

    public void ApplySpeedUpgrade(float multiplier)
    {
        moveSpeed *= multiplier;
    }
}
