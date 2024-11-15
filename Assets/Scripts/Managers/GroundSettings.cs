// Scripts/Managers/GroundSettings.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewGroundSettings", menuName = "Game/GroundSettings")]
public class GroundSettings : ScriptableObject
{
    [Header("Ground Configuration")]
    public GameObject groundTilePrefab; // Prefab for ground tiles
    public int groundWidth; // Number of tiles horizontally
    public int groundLength; // Number of tiles vertically
}
