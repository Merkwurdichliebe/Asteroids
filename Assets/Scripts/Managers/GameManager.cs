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

        Instantiate(PrefabSpawnSafeZone, Vector2.zero, Quaternion.identity);
        UIManager uiManager = GetComponent<UIManager>();
        if (uiManager != null) uiManager.enabled = true;
        player = Instantiate(player);
    }



    void OnEnable()
    {
        EventManager.Instance.OnUFODestroyed += StartUFOSpawner;
        EventManager.Instance.OnUFODestroyed += CheckLevelCleared;
        EventManager.Instance.OnAsteroidDestroyed += CheckLevelCleared;
        EventManager.Instance.OnEntityKilledByPlayer += HandleScorePoints;
        EventManager.Instance.OnPlayerLivesZero += Cleanup;
    }



    void OnDisable()
    {
        EventManager.Instance.OnUFODestroyed -= StartUFOSpawner;
        EventManager.Instance.OnUFODestroyed += CheckLevelCleared;
        EventManager.Instance.OnAsteroidDestroyed -= CheckLevelCleared;
        EventManager.Instance.OnEntityKilledByPlayer -= HandleScorePoints;
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
        UIManager.Instance.DisplayLevelNumber(level);
        yield return new WaitForSeconds(3);
        UIManager.Instance.DisplayGameUI();
        StartNextLevel();
    }



    void StartNextLevel()
    {
        SpawnAsteroids();
        player.Spawn();
        StartUFOSpawner();
    }



    void SpawnAsteroids()
    {
        // Spawn asteroids based on level number
        for (int i = 0; i < startingAsteroids + level - 1; i++)
        {
            Instantiate(PrefabAsteroid, Vector2.zero, Quaternion.identity);
        }
    }



    private void StartUFOSpawner()
    {
        Debug.Log("[GameManager/StartUFOSpawner]");
        if (UFOController.Count < 1)
        {
            Debug.Log("(UFO.Count = " + UFOController.Count + ")");
            StartCoroutine(SpawnUFO());
        }
    }



    IEnumerator SpawnUFO()
    {
        // Allow at least 2 seconds between death and respawn
        yield return new WaitForSeconds(3.0f);

        // If the random value is higher than the probability,
        // wait some more (spawnFrequency in seconds)
        while (Random.value > UFOSpawnProbability)
        {
            yield return new WaitForSeconds(UFOSpawnFrequency);
        }

        // Spawn the UFO
        Instantiate(PrefabUFO);
        Debug.Log("[GameManager/SpawnUFO] Spawned");
    }



    void CheckLevelCleared()
    {
        if (AsteroidController.Count == 0 && UFOController.Count == 0)
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
