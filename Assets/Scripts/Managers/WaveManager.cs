using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveData[] waves;
    public Transform playerTransform;
    public float waveDelay = 5f;

    public float spawnAreaWidth = 20f;
    public float spawnAreaLength = 20f;

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

        if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
        {
            spawnPosition.y = hit.point.y;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Set the ground bounds for the enemy's movement constraints
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetGroundBounds(groundBounds);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
        float z = Random.Range(-spawnAreaLength / 2, spawnAreaLength / 2);
        Vector3 spawnPosition = new Vector3(x, 0, z);

        return spawnPosition;
    }

    public void SetGroundBounds(Bounds bounds)
    {
        groundBounds = bounds;
    }
}
