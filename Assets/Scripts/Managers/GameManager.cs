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
    public GameObject PrefabAsteroid;
    public GameObject PrefabUFO;
    public GameObject PrefabPowerup;
    public GameObject PrefabSpawnSafeZone; 

    // Reference to the PlayerController script
    public PlayerController player;

    // Tunable game data
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
        player = Instantiate(player, Vector2.zero, Quaternion.identity);
    }



    void OnEnable()
    {
        EventManager.Instance.OnLevelCleared += PrepareNextLevel;
        EventManager.Instance.OnUFODestroyed += StartUFOSpawner;
        EventManager.Instance.OnUFODestroyed += CheckLevelCleared;
        EventManager.Instance.OnAsteroidDestroyed += CheckLevelCleared;
        EventManager.Instance.OnEntityKilledByPlayer += HandleScorePoints;
        EventManager.Instance.OnPlayerLivesZero += Cleanup;
    }



    void OnDisable()
    {
        EventManager.Instance.OnLevelCleared -= PrepareNextLevel;
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
        player.SetAlive(false);
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

        // Spawn UFO and add the player object as its target
        GameObject ufo = Instantiate(PrefabUFO);

        // FIXME this gets a null reference when gameover
        ufo.GetComponent<ICanFireAtTarget>().Target = player.gameObject;
        Debug.Log("[GameManager/SpawnUFO] Spawned");

    }



    void CheckLevelCleared()
    {
        if (AsteroidController.Count == 0 && UFOController.Count == 0)
        {
            EventManager.Instance.LevelCleared();
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
// TODO: clean player spawning routine
// TODO: fix screen aspect and wrap around issues
// TODO: end level only when UFO not here
// TODO: reset player acceleration sprite when respawning
// TODO: more asteroids sprite variations
