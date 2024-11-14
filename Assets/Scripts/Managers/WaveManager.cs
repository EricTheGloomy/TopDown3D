// Scripts/Managers/WaveManager.cs
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveData[] waves;
    public Transform playerTransform;
    public float waveDelay = 5f;

    public float spawnAreaWidth = 20f; // Define play area width
    public float spawnAreaLength = 20f; // Define play area length

    private int currentWaveIndex = 0;

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

        // Raycast down to find the ground level, adjust position if needed
        if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
        {
            spawnPosition.y = hit.point.y;
        }

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Generate a random position within the spawn area boundaries
        float x = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
        float z = Random.Range(-spawnAreaLength / 2, spawnAreaLength / 2);
        Vector3 spawnPosition = new Vector3(x, 0, z);

        return spawnPosition;
    }
}
