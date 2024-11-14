// Scripts/Managers/ObstacleManager.cs
using UnityEngine;

public class ObstacleManager
{
    private Bounds groundBounds;
    private float groundYLevel;
    private PrefabSettings obstacleSettings;

    public ObstacleManager(PrefabSettings obstacleSettings, GroundManager groundManager)
    {
        this.obstacleSettings = obstacleSettings;
        groundBounds = groundManager.GetGroundBounds();
        groundYLevel = groundManager.GetGroundYLevel();

        PlaceObstacles();
    }

    private void PlaceObstacles()
    {
        int obstacleCount = Random.Range(15, 25);
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 position = GetRandomPositionInBounds(groundBounds, margin: 2f);
            position.y = groundYLevel + obstacleSettings.yOffset; // Use yOffset from PrefabSettings
            Object.Instantiate(obstacleSettings.prefab, position, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPositionInBounds(Bounds bounds, float margin)
    {
        float x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
        float z = Random.Range(bounds.min.z + margin, bounds.max.z - margin);
        return new Vector3(x, groundYLevel + obstacleSettings.yOffset, z); // Apply yOffset
    }
}
