// Scripts/Managers/GroundManager.cs
using UnityEngine;

public class GroundManager
{
    public GameObject GroundInstance { get; private set; }
    private Bounds groundBounds;
    private float groundYLevel;

    public GroundManager(GameObject groundPrefab)
    {
        GroundInstance = Object.Instantiate(groundPrefab);
        groundBounds = GroundInstance.GetComponent<Renderer>().bounds;
        groundYLevel = groundBounds.center.y; // Set exact ground level
    }

    public Bounds GetGroundBounds()
    {
        return groundBounds;
    }

    public float GetGroundYLevel()
    {
        return groundYLevel;
    }
}
