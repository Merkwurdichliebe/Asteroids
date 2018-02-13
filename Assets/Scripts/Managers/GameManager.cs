using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //
    // Inspector fields
    //
    [Header("Main game prefabs")]
    public PlayerController playerPrefab;
    public CloneWhenKilled asteroidPrefab;

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
    public bool playLevelIntro;

    //
    // Private fields
    //
    private int countAsteroids;
    private Spawner spawner;
    private GameObject spawnSafeZone;
    private Transform asteroidsParent;
    private readonly Vector2 halfUnit = new Vector2(0.5f, 0.5f);
    private Camera cam;

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

        asteroidsParent = new GameObject().transform;
        asteroidsParent.gameObject.name = "Asteroids";

        cam = Camera.main;
    }



    void OnEnable()
    {
        KeepInstancesCount.OnLastDestroyed += CheckLastDestroyed;
        PlayerController.OnPlayerLivesZero += Cleanup;
        PlayerController.OnPlayerSpawned += DisableSafeZone;
        PlayerController.OnPlayerDestroyed += EnableSafeZone;
        PlayerController.OnPlayerDespawned += EnableSafeZone;
    }



    void OnDisable()
    {
        PlayerController.OnPlayerLivesZero -= Cleanup;
        KeepInstancesCount.OnLastDestroyed -= CheckLastDestroyed;
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
        if (playLevelIntro)
            StartCoroutine(ReadyNextLevel());    
        else
            StartNextLevel();
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
    // Spawn the first asteroids.
    // Set the SourcePrefab property to point to the asteroidPrefab used here,
    // so that an asteroid can clone itself.
    // Get a random vector inside a unit circle,
    // shift it to the center of the viewport (0.5, 0.5)
    // and scale it down so that it draws a circle around the player.
    //
    public void SpawnAsteroids(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Instantiate
            CloneWhenKilled asteroid = Instantiate(asteroidPrefab, Vector2.zero, Quaternion.identity, asteroidsParent.transform);
            asteroid.SourcePrefab = asteroidPrefab;

            // Set position
            Vector2 pos = Random.insideUnitCircle.normalized + halfUnit;
            Vector3 worldPos = cam.ViewportToWorldPoint(pos) / 2;
            worldPos.z = 0;
            asteroid.gameObject.transform.position = worldPos;
        }
    }

    public void CheckLastDestroyed(KeepInstancesCount component)
    {
        if(component.gameObject.tag.Equals("Asteroid"))
        {
            CheckLevelCleared();
        }
    }

    void CheckLevelCleared()
    {
        if (Player != null)
        {
            // Debug.Log("[GameManager/CheckLevelCleared] Asteroids " +
            //         AsteroidController.Count + " Spawner " + spawner.TotalCount);
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