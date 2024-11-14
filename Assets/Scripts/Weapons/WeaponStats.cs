// Scripts/Weapons/WeaponStats.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Game/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public float damage = 10f;
    public float range = 10f;
    public float fireRate = 1f; // Shots per second
    public float projectileSpeed = 10f;
}
