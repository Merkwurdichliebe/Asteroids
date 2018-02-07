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

    // Reference to the PlayerController script
    public PlayerController player;

    // Tunable game data
    [Header("Game options")]
    public int startingAsteroids;
    public static int level;
    public float UFOSpawnFrequency;
    public float UFOSpawnProbability;

    // -------------------------------------------------------------------------
    // Private variables and properties
    // -------------------------------------------------------------------------

    private int playerScore;
    public int PlayerScore { get { return playerScore; } }

    private int countUFO;
    private int countAsteroids;
    private Spawner spawner;

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
        level = 0;

        // Create the Player spawn safe zone
        Instantiate(PrefabSpawnSafeZone, Vector2.zero, Quaternion.identity);
        UIManager uiManager = GetComponent<UIManager>();
        if (uiManager != null) uiManager.enabled = true;

        // Get a reference to the Spawner
        spawner = GetComponent<Spawner>();
        if (spawner == null)
        {
            Debug.LogWarning("[GameManager] No Spawner present. Entity spawning disabled.");
        }

        player = Instantiate(player);
    }



    void OnEnable()
    {
        //EventManager.Instance.OnAsteroidDestroyed += CheckLevelCleared;
        EventManager.Instance.OnEntityKilledByPlayer += HandleScorePoints;
        EventManager.Instance.OnEntityKilledByPlayer += CheckLevelCleared;
        EventManager.Instance.OnPlayerLivesZero += Cleanup;
    }



    void OnDisable()
    {
        //EventManager.Instance.OnAsteroidDestroyed -= CheckLevelCleared;
        EventManager.Instance.OnEntityKilledByPlayer -= HandleScorePoints;
        EventManager.Instance.OnEntityKilledByPlayer -= CheckLevelCleared;
        EventManager.Instance.OnPlayerLivesZero -= Cleanup;
    }

    // -----------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------

    void Start()
    {
        PrepareNextLevel();
    }



    void PrepareNextLevel()
    {
        Debug.Log("[GameManager/PrepareNextLevel] " + (level + 1));
        StopAllCoroutines();
        StartCoroutine(ReadyNextLevel());
    }



    IEnumerator ReadyNextLevel()
    {
        level += 1;
        player.ActiveInScene = false;
        if (spawner != null) { spawner.StopSpawning(); }
        UIManager.Instance.DisplayLevelNumber(level);
        yield return new WaitForSeconds(3);
        UIManager.Instance.DisplayGameUI();
        StartNextLevel();
    }



    void StartNextLevel()
    {
        SpawnAsteroids();
        player.Spawn();
        if (spawner != null) { spawner.StartSpawning(); }
    }



    void SpawnAsteroids()
    {
        // Spawn asteroids based on level number
        for (int i = 0; i < startingAsteroids + level - 1; i++)
        {
            Instantiate(PrefabAsteroid, Vector2.zero, Quaternion.identity);
        }
    }

    void CheckLevelCleared(ICanScorePoints e)
    {
        Debug.Log("[GameManager/CheckLevelCleared] Asteroids " +
                  AsteroidController.Count + " Spawner " + spawner.TotalCount);
        if (AsteroidController.Count == 0 && spawner.TotalCount == 0)
        {
            PrepareNextLevel();
        }
    }



    void HandleScorePoints(ICanScorePoints e)
    {
        playerScore += e.PointValue;
        EventManager.Instance.GameScoreChanged(playerScore);
    }



    void Cleanup()
    {
        StopAllCoroutines();
    }
}

// TODO: powerups
// TODO: reset player acceleration sprite when respawning
// TODO: more asteroids sprite variations
