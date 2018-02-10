using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Inspector variables
    // -------------------------------------------------------------------------

    // References to prefabs
    [Header("Main game prefabs")]
    public PlayerController playerPrefab;
    public AsteroidController asteroidPrefab;

    [Header("Extra game prefabs")]
    public GameObject spawnSafeZonePrefab;
    public GameObject cometPrefab;

    // Tunable game data
    [Header("Game options")]
    public int startWithLevel = 1;
    public int startWithAsteroids = 3;
    public int startWithPlayerLives = 3;
    public bool spawnAsteroids;
    public bool spawnPlayer;

    // -------------------------------------------------------------------------
    // Private variables and properties
    // -------------------------------------------------------------------------

    private int countAsteroids;
    private Spawner spawner;
    private GameObject spawnSafeZone;

    // 
    // Properties
    //

    public PlayerController Player { get; private set; }

    public static int CurrentLevel
    {
        get; private set;
    }

    //
    //  Events
    //

    public static Action OnGameLevelDisplay;
    public static Action OnGameLevelStart;

    // -------------------------------------------------------------------------
    // Setup
    // -------------------------------------------------------------------------


    void Awake()
    {
        // Check for unconnected prefabs
        Assert.IsNotNull(asteroidPrefab);
        Assert.IsNotNull(playerPrefab);
        Assert.IsNotNull(spawnSafeZonePrefab);

        // Set the level number
        CurrentLevel = startWithLevel;

        // Create the Player spawn safe zone
        spawnSafeZone = Instantiate(spawnSafeZonePrefab, Vector2.zero, Quaternion.identity);
        UIManager ui = GetComponent<UIManager>();
        if (ui != null) ui.enabled = true;

        // Get a reference to the Spawner
        spawner = GetComponent<Spawner>();
        if (spawner == null)
        {
            Debug.LogWarning("[GameManager/Awake] No Spawner present. Entity spawning disabled.");
        }

        if (spawnPlayer)
        {
            Player = Instantiate(playerPrefab);
            Player.Lives = startWithPlayerLives;
        }
    }



    void OnEnable()
    {
        AsteroidController.OnAsteroidLastDestroyed += CheckLevelCleared;
        PlayerController.OnPlayerLivesZero += Cleanup;
        PlayerController.OnPlayerSpawned += DisableSafeZone;
        PlayerController.OnPlayerDestroyed += EnableSafeZone;
        PlayerController.OnPlayerDespawned += EnableSafeZone;
    }



    void OnDisable()
    {
        PlayerController.OnPlayerLivesZero -= Cleanup;
        AsteroidController.OnAsteroidLastDestroyed -= CheckLevelCleared;
        PlayerController.OnPlayerSpawned -= DisableSafeZone;
        PlayerController.OnPlayerDestroyed -= EnableSafeZone;
        PlayerController.OnPlayerDespawned -= EnableSafeZone;
    }

    // -----------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------

    void Start()
    {
        PrepareNextLevel();
    }

    //
    // 
    //
    void DisableSafeZone()
    {
        spawnSafeZone.SetActive(false);
    }

    void EnableSafeZone()
    {
        spawnSafeZone.SetActive(true);
    }

    //
    // 
    //
    void PrepareNextLevel()
    {
        Debug.Log("[GameManager/PrepareNextLevel] " + CurrentLevel);
        StopAllCoroutines();
        StartCoroutine(ReadyNextLevel());
    }

    //
    // 
    //
    IEnumerator ReadyNextLevel()
    {
        if (OnGameLevelDisplay != null) { OnGameLevelDisplay(); }
        if (Player != null) { Player.ActiveInScene = false; }
        UIManager.Instance.DisplayLevelNumber(CurrentLevel);
        Instantiate(cometPrefab);
        yield return new WaitForSeconds(3);
        if (OnGameLevelStart != null) { OnGameLevelStart(); }
        UIManager.Instance.DisplayGameUI();
        StartNextLevel();
    }


    //
    // 
    //
    void StartNextLevel()
    {
        if (spawnAsteroids) {
            SpawnAsteroids(startWithAsteroids + CurrentLevel - 1);
        }
        if (spawnPlayer) {
            Player.SpawnInSeconds(0);
        }
    }

    //
    // 
    //
    public void SpawnAsteroids(int count)
    {
        // Spawn asteroids based on level number
        for (int i = 0; i < count; i++)
        {
            Instantiate(asteroidPrefab, Vector2.zero, Quaternion.identity);
        }
    }

    void CheckLevelCleared()
    {
        if (Player != null)
        {
            Debug.Log("[GameManager/CheckLevelCleared] Asteroids " +
                      AsteroidController.Count + " Spawner " + spawner.TotalCount);
            if (Player.Lives != 0)
            {
                CurrentLevel += 1;
                PrepareNextLevel();
            }
        }
    }

    void Cleanup()
    {
        StopAllCoroutines();
    }
}

// TODO: powerups
// TODO: more asteroids sprite variations
