// Scripts/Managers/CameraManager.cs
using UnityEngine;

public class CameraManager
{
    private GameObject cameraInstance;

    public CameraManager(GameObject cameraPrefab, Transform playerTransform)
    {
        cameraInstance = Object.Instantiate(cameraPrefab);
        CameraFollow cameraFollow = cameraInstance.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.playerTransform = playerTransform;
        }
    }
}
