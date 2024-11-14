// Scripts/Managers/GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject groundPrefab;
    public PrefabSettings obstacleSettings; // Use PrefabSettings for obstacle
    public PrefabSettings playerSettings;   // Use PrefabSettings for player
    public GameObject cameraPrefab;
    public GameObject waveManagerPrefab;

    private GroundManager groundManager;
    private ObstacleManager obstacleManager;
    private PlayerManager playerManager;
    private CameraManager cameraManager;
    private WaveManager waveManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        Debug.Log("Game Initialized");

        // Step 1: Initialize ground
        groundManager = new GroundManager(groundPrefab);

        // Step 2: Initialize obstacles using PrefabSettings
        obstacleManager = new ObstacleManager(obstacleSettings, groundManager);

        // Step 3: Initialize player using PrefabSettings
        playerManager = new PlayerManager(playerSettings, groundManager, obstacleManager);

        // Step 4: Initialize camera
        cameraManager = new CameraManager(cameraPrefab, playerManager.PlayerInstance.transform);

        // Step 5: Initialize wave manager
        waveManager = Instantiate(waveManagerPrefab).GetComponent<WaveManager>();
        waveManager.playerTransform = playerManager.PlayerInstance.transform;
    }
}
