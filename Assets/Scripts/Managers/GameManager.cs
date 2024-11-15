// Scripts/Managers/GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Ground Settings")]
    public GroundSettings groundSettings; // ScriptableObject for ground configuration

    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs; // Obstacle prefabs
    public PrefabSettings obstacleSettings;

    [Header("Player Settings")]
    public PrefabSettings playerSettings;

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
        groundManager = new GroundManager(groundSettings);

        // Step 2: Initialize obstacles
        obstacleManager = new ObstacleManager(obstaclePrefabs, obstacleSettings, groundManager);

        // Step 3: Initialize player
        playerManager = new PlayerManager(playerSettings, groundManager, obstacleManager);

        // Step 4: Set ground bounds for player
        var playerController = playerManager.PlayerInstance.GetComponent<PlayerController>();
        playerController?.SetGroundBounds(groundManager.GetGroundBounds());

        // Step 5: Initialize camera
        cameraManager = new CameraManager(cameraPrefab, playerManager.PlayerInstance.transform);

        // Step 6: Initialize wave manager
        waveManager = Instantiate(waveManagerPrefab).GetComponent<WaveManager>();
        waveManager.playerTransform = playerManager.PlayerInstance.transform;
        waveManager.SetGroundBounds(groundManager.GetGroundBounds());
    }
}
