// Scripts/Player/PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;

    private CharacterController characterController;
    private float moveSpeed; // Local instance move speed

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        moveSpeed = playerStats.moveSpeed; // Initialize from PlayerStats
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection != Vector3.zero)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            transform.forward = moveDirection;
        }
    }

    public void ApplySpeedUpgrade(float multiplier)
    {
        moveSpeed *= multiplier; // Upgrade only affects this instance
    }
}
