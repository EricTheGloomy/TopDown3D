// Scripts/Managers/WaveData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Game/WaveData")]
public class WaveData : ScriptableObject
{
    [Tooltip("Enemy prefab to spawn in this wave")]
    public GameObject enemyPrefab;

    [Tooltip("Number of enemies to spawn in this wave")]
    public int enemyCount = 5;

    [Tooltip("Time interval between enemy spawns")]
    public float spawnInterval = 1f;
}
