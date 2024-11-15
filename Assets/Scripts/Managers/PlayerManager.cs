// Scripts/Managers/PlayerManager.cs
using UnityEngine;

public class PlayerManager
{
    public GameObject PlayerInstance { get; private set; }
    private Bounds groundBounds;
    private GroundManager groundManager;
    private PrefabSettings playerSettings;

    public PlayerManager(PrefabSettings playerSettings, GroundManager groundManager, ObstacleManager obstacleManager)
    {
        this.playerSettings = playerSettings;
        this.groundManager = groundManager;
        groundBounds = groundManager.GetGroundBounds();

        Vector3 spawnPosition = GetValidPlayerPosition();
        spawnPosition.y = GetAdjustedHeight(spawnPosition); // Corrected height adjustment
        PlayerInstance = Object.Instantiate(playerSettings.prefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetValidPlayerPosition()
    {
        Vector3 position;
        bool isValid;
        do
        {
            position = GetRandomPositionInBounds(groundBounds, margin: 2f);
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
                return false; // Ensure player does not overlap obstacles
            }
        }
        return true;
    }

    private Vector3 GetRandomPositionInBounds(Bounds bounds, float margin)
    {
        float x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
        float z = Random.Range(bounds.min.z + margin, bounds.max.z - margin);
        return new Vector3(x, 0, z); // Y will be determined dynamically
    }

    private float GetAdjustedHeight(Vector3 position)
    {
        // Get ground height at the position
        float groundHeight = groundManager.GetGroundHeightAt(position);

        // Get player's collider height
        GameObject tempPlayer = Object.Instantiate(playerSettings.prefab, position, Quaternion.identity);
        Collider playerCollider = tempPlayer.GetComponent<Collider>();
        float colliderHeight = 0f;

        if (playerCollider != null)
        {
            colliderHeight = playerCollider.bounds.extents.y; // Half the height of the collider
        }

        Object.Destroy(tempPlayer); // Cleanup temporary instance
        return groundHeight + colliderHeight;
    }
}
