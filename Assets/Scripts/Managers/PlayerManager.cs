// Scripts/Managers/PlayerManager.cs
using UnityEngine;

public class PlayerManager
{
    public GameObject PlayerInstance { get; private set; }
    private Bounds groundBounds;
    private float groundYLevel;
    private PrefabSettings playerSettings;
    private ObstacleManager obstacleManager;

    public PlayerManager(PrefabSettings playerSettings, GroundManager groundManager, ObstacleManager obstacleManager)
    {
        this.playerSettings = playerSettings;
        groundBounds = groundManager.GetGroundBounds();
        groundYLevel = groundManager.GetGroundYLevel();
        this.obstacleManager = obstacleManager;

        Vector3 spawnPosition = GetValidPlayerPosition();
        spawnPosition.y = groundYLevel + playerSettings.yOffset; // Use yOffset from PrefabSettings
        PlayerInstance = Object.Instantiate(playerSettings.prefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetValidPlayerPosition()
    {
        Vector3 position;
        bool isValid;
        do
        {
            position = GetRandomPositionInBounds(groundBounds, margin: 2f);
            position.y = groundYLevel + playerSettings.yOffset; // Set Y using yOffset
            isValid = CheckPositionIsClear(position);
        } while (!isValid);

        return position;
    }

    private bool CheckPositionIsClear(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                return false; // Avoid overlapping with obstacles
            }
        }
        return true; // Position is clear
    }

    private Vector3 GetRandomPositionInBounds(Bounds bounds, float margin)
    {
        float x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
        float z = Random.Range(bounds.min.z + margin, bounds.max.z - margin);
        return new Vector3(x, groundYLevel + playerSettings.yOffset, z); // Apply yOffset
    }
}
