// Scripts/Managers/PrefabSettings.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewPrefabSettings", menuName = "Game/PrefabSettings")]
public class PrefabSettings : ScriptableObject
{
    public GameObject prefab;
    public float yOffset;
}
