// Scripts/Managers/GroundManager.cs
using UnityEngine;

public class GroundManager
{
    public GameObject GroundInstance { get; private set; }
    private Bounds groundBounds;

    private GroundSettings groundSettings;
    private float tileSize; // Automatically determines tile size

    public GroundManager(GroundSettings groundSettings)
    {
        this.groundSettings = groundSettings;
        this.tileSize = CalculateTileSize(); // Dynamically calculate the tile size
        GenerateGround();
    }

    private float CalculateTileSize()
    {
        // Use the bounds of the prefab's Renderer to determine tile size
        Renderer renderer = groundSettings.groundTilePrefab.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size.x; // Assuming square tiles
        }
        return 10f; // Default to 10 if no renderer is available
    }

    private void GenerateGround()
    {
        GameObject parentObject = new GameObject("GroundParent");

        Vector3 minBound = Vector3.positiveInfinity;
        Vector3 maxBound = Vector3.negativeInfinity;

        for (int x = 0; x < groundSettings.groundWidth; x++)
        {
            for (int z = 0; z < groundSettings.groundLength; z++)
            {
                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize); // Use calculated tile size
                GameObject tile = Object.Instantiate(groundSettings.groundTilePrefab, position, Quaternion.identity, parentObject.transform);

                // Dynamically calculate bounds based on instantiated tiles
                Renderer renderer = tile.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Bounds tileBounds = renderer.bounds;
                    minBound = Vector3.Min(minBound, tileBounds.min);
                    maxBound = Vector3.Max(maxBound, tileBounds.max);
                }
            }
        }

        groundBounds = new Bounds((minBound + maxBound) / 2, maxBound - minBound);
    }

    public Bounds GetGroundBounds()
    {
        return groundBounds;
    }

    public float GetGroundHeightAt(Vector3 position)
    {
        Ray ray = new Ray(new Vector3(position.x, 100f, position.z), Vector3.down); // Cast from above
        if (Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            return hit.point.y; // Return the Y position of the ground
        }
        return 0f; // Default height if no ground detected
    }
}
