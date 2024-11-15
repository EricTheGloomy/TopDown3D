// Scripts/Managers/ObstacleManager.cs
using UnityEngine;

public class ObstacleManager
{
    private Bounds groundBounds;
    private GroundManager groundManager;
    private GameObject[] obstaclePrefabs;
    private PrefabSettings obstacleSettings; // For yOffset
    public int obstacleCount = 20;

    // Updated constructor to accept PrefabSettings
    public ObstacleManager(GameObject[] obstaclePrefabs, PrefabSettings obstacleSettings, GroundManager groundManager)
    {
        this.obstaclePrefabs = obstaclePrefabs;
        this.obstacleSettings = obstacleSettings; // Store settings for yOffset
        this.groundManager = groundManager;
        groundBounds = groundManager.GetGroundBounds();

        PlaceObstacles();
    }

    private void PlaceObstacles()
    {
        int placedObstacles = 0;
        while (placedObstacles < obstacleCount)
        {
            Vector3 position = GetRandomPositionInBounds(groundBounds, margin: 2f);
            position.y = groundManager.GetGroundHeightAt(position) + obstacleSettings.yOffset; // Adjust height with yOffset

            if (CheckPositionIsClear(position))
            {
                GameObject prefab = GetRandomObstaclePrefab();
                Object.Instantiate(prefab, position, Quaternion.identity);
                placedObstacles++;
            }
        }
    }

    private GameObject GetRandomObstaclePrefab()
    {
        return obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
    }

    private bool CheckPositionIsClear(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                return false;
            }
        }
        return true;
    }

    private Vector3 GetRandomPositionInBounds(Bounds bounds, float margin)
    {
        float x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
        float z = Random.Range(bounds.min.z + margin, bounds.max.z - margin);
        return new Vector3(x, 0, z); // Y will be determined by ground height
    }
}
