// Scripts/Player/PlayerStats.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Game/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float health = 100f;
    public float moveSpeed = 5f;
    public float pickupRadius = 2f;
}
