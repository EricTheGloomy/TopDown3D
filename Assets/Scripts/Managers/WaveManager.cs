// Scripts/Managers/WaveManager.cs
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveData[] waves;
    public Transform playerTransform;
    public float waveDelay = 5f;

    public float spawnAreaWidth = 20f;
    public float spawnAreaLength = 20f;
    public LayerMask groundLayer;
    private int currentWaveIndex = 0;
    private Bounds groundBounds;

    private void Start()
    {
        StartCoroutine(StartWaveRoutine());
    }

    private IEnumerator StartWaveRoutine()
    {
        while (currentWaveIndex < waves.Length)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            yield return new WaitForSeconds(waveDelay);
            currentWaveIndex++;
        }

        Debug.Log("All waves completed!");
    }

    private IEnumerator SpawnWave(WaveData waveData)
    {
        Debug.Log("Starting wave: " + currentWaveIndex);

        for (int i = 0; i < waveData.enemyCount; i++)
        {
            SpawnEnemy(waveData.enemyPrefab);
            yield return new WaitForSeconds(waveData.spawnInterval);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Adjust the spawn position to align with the ground
        spawnPosition.y = GetAdjustedHeight(spawnPosition);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Log the spawn position
        Debug.Log($"Spawned enemy {enemy.name} at position: {spawnPosition}");

        // Ensure ground bounds are set for the enemy
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetGroundBounds(groundBounds);
        }

        // Adjust position after instantiation to ensure it's not submerged
        AdjustEnemyHeight(enemy);
    }

    private void AdjustEnemyHeight(GameObject enemy)
    {
        Vector3 position = enemy.transform.position;

        // Raycast down from above the enemy to detect the ground
        Ray ray = new Ray(new Vector3(position.x, 100f, position.z), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundLayer))
        {
            // Adjust the Y position based on the raycast hit point
            position.y = hit.point.y;

            // Debugging: Confirm the raycast hit
            Debug.Log($"{enemy.name} detected ground at Y = {hit.point.y}");
        }
        else
        {
            // If no ground detected, default to Y = 0
            Debug.LogWarning($"{enemy.name} could not detect ground. Defaulting Y = 0.");
            position.y = 0f;
        }

        // Apply the adjusted position
        enemy.transform.position = position;

        // Adjust based on collider height after position is set
        Collider enemyCollider = enemy.GetComponent<Collider>();
        if (enemyCollider != null)
        {
            // Move the enemy up by half the collider height
            position.y += enemyCollider.bounds.extents.y;
            enemy.transform.position = position;
        }

        // Debugging: Confirm the final position
        Debug.Log($"{enemy.name} final adjusted position: {position}");
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(groundBounds.min.x, groundBounds.max.x);
        float z = Random.Range(groundBounds.min.z, groundBounds.max.z);
        return new Vector3(x, 0, z); // Y will be adjusted later
    }

    public void SetGroundBounds(Bounds bounds)
    {
        groundBounds = bounds;
    }

    private float GetAdjustedHeight(Vector3 position)
    {
        // Get ground height at the position using a raycast
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(position.x, 100f, position.z), Vector3.down, out hit, 200f, groundLayer))
        {
            // Return the ground height based on the raycast hit point
            Debug.Log($"Ground height detected at position {position}: {hit.point.y}");
            return hit.point.y;
        }
        else
        {
            // If no ground detected, return 0 as a fallback
            Debug.LogWarning($"No ground detected at position {position}, defaulting to height 0.");
            return 0f;
        }
    }
}
