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
    public GameObject PrefabAsteroid;
    public GameObject PrefabUFO;
    public GameObject PrefabPowerup;
    public GameObject PrefabSpawnSafeZone;
    public GameObject PrefabComet;

    // Reference to the PlayerController script
    public PlayerController player;

    // Tunable game data
    [Header("Game options")]
    public int startWithLevel = 1;
    public int startWithAsteroids = 3;
    public int startWithPlayerLives = 3;
    public bool dontSpawnAsteroids;
    public bool dontSpawnPlayer;

    // -------------------------------------------------------------------------
    // Private variables and properties
    // -------------------------------------------------------------------------

    private int countAsteroids;
    private Spawner spawner;
    private GameObject spawnSafeZone;

    // 
    // Properties
    //

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
        Assert.IsNotNull(PrefabAsteroid);
        Assert.IsNotNull(PrefabUFO);
        Assert.IsNotNull(player);

        // Set the level number
        CurrentLevel = startWithLevel;

        // Create the Player spawn safe zone
        spawnSafeZone = Instantiate(PrefabSpawnSafeZone, Vector2.zero, Quaternion.identity);
        UIManager ui = GetComponent<UIManager>();
        if (ui != null) ui.enabled = true;

        // Get a reference to the Spawner
        spawner = GetComponent<Spawner>();
        if (spawner == null)
        {
            Debug.LogWarning("[GameManager/Awake] No Spawner present. Entity spawning disabled.");
        }

        player = Instantiate(player);
        player.Lives = startWithPlayerLives;
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

    void DisableSafeZone()
    {
        spawnSafeZone.SetActive(false);
    }

    void EnableSafeZone()
    {
        spawnSafeZone.SetActive(true);
    }

    void PrepareNextLevel()
    {
        Debug.Log("[GameManager/PrepareNextLevel] " + CurrentLevel);
        StopAllCoroutines();
        StartCoroutine(ReadyNextLevel());
    }



    IEnumerator ReadyNextLevel()
    {
        if (OnGameLevelDisplay != null) { OnGameLevelDisplay(); }
        player.ActiveInScene = false;
        UIManager.Instance.DisplayLevelNumber(CurrentLevel);
        Instantiate(PrefabComet);
        yield return new WaitForSeconds(3);
        if (OnGameLevelStart != null) { OnGameLevelStart(); }
        UIManager.Instance.DisplayGameUI();
        StartNextLevel();
    }



    void StartNextLevel()
    {
        if (!dontSpawnAsteroids)
        {
            SpawnAsteroids();
        }
        if (!dontSpawnPlayer)
        {
            player.SpawnInSeconds(0);
        }
    }



    void SpawnAsteroids()
    {
        // Spawn asteroids based on level number
        for (int i = 0; i < startWithAsteroids + CurrentLevel - 1; i++)
        {
            Instantiate(PrefabAsteroid, Vector2.zero, Quaternion.identity);
        }
    }

    void CheckLevelCleared()
    {
        Debug.Log("[GameManager/CheckLevelCleared] Asteroids " +
                  AsteroidController.Count + " Spawner " + spawner.TotalCount);
        if (player.Lives != 0) {
            CurrentLevel += 1;
            PrepareNextLevel();
        }
    }

    void Cleanup()
    {
        StopAllCoroutines();
    }
}

// TODO: powerups
// TODO: more asteroids sprite variations
