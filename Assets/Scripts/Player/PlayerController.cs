// Scripts/Player/PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;
    private CharacterController characterController;
    private float moveSpeed;
    private Bounds groundBounds;

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
        
        // Clamp the player's position within the ground bounds
        position.x = Mathf.Clamp(position.x, groundBounds.min.x, groundBounds.max.x);
        position.z = Mathf.Clamp(position.z, groundBounds.min.z, groundBounds.max.z);

        transform.position = position;
    }

    public void SetGroundBounds(Bounds bounds)
    {
        groundBounds = bounds;
    }

    public void ApplySpeedUpgrade(float multiplier)
    {
        moveSpeed *= multiplier;
    }
}
