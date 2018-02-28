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
    // [Header("Main game prefabs")]
    // public PlayerController playerPrefab;

    [Header("Extra game prefabs")]
    
    public GameObject cometPrefab;

    // Tunable game data
    public GameSettings gameSettings;

    //
    // Private fields
    //
    private int countAsteroids;
    private AsteroidSpawner asteroidSpawner;
    private Spawner spawner;
    private MusicManager musicManager;

    // 
    // Properties
    //
    // public PlayerController Player { get; private set; }

    public static int CurrentLevel
    {
        get; private set;
    }

    //
    //  Events
    //
    public static Action OnGameLevelReady;
    public static Action OnGameLevelStart;

    //
    // Initialisation
    //
    void Awake()
    {
        // Set the level number
        CurrentLevel = gameSettings.level;



        // Get a reference to the spawners
        asteroidSpawner = GetComponent<AsteroidSpawner>();
        spawner = GetComponent<Spawner>();
        musicManager = GetComponent<MusicManager>();

        // Spawn the player
        // if (gameSettings.spawnPlayer)
        // {
        //     Player = Instantiate(playerPrefab);
        //     Player.Lives = gameSettings.lives;
        // }

        musicManager.enabled = gameSettings.playMusic;
    }

    //
    // Start the game sequence
    //
    void Start()
    {
        spawner.enabled = gameSettings.spawnOthers;
        PrepareNextLevel();
    }

    //
    // Do the level intro sequence if needed,
    // otherwise start the level directly.
    //
    void PrepareNextLevel()
    {
        StopAllCoroutines();

        if (gameSettings.playIntro)
            StartCoroutine(ReadyNextLevel());    
        else
            StartNextLevel();
    }

    // Level intro sequence
    //
    IEnumerator ReadyNextLevel()
    {
        // Send message
        if (OnGameLevelReady != null) { OnGameLevelReady(); }

        // Deactivate the player & safe zone
        // if (Player != null) 
        //     Player.GetComponent<EntitySpawnController>().ActiveInScene = false;
        
        // This needs to come after player deactivation or it will reactivate
        // spawnSafeZone.SetActive(false);

        // Animate the comet effect, wait for 3 seconds and start the level
        Instantiate(cometPrefab);
        yield return new WaitForSeconds(3);
        StartNextLevel();
    }

    //
    // Start the level
    //
    void StartNextLevel()
    {
        // Send message
        if (OnGameLevelStart != null) { OnGameLevelStart(); }

        // Spawn asteroids
        if (gameSettings.spawnAsteroids)
            asteroidSpawner.Spawn(gameSettings.asteroids + CurrentLevel - 1);
    
        // // Spawn the player
        // if (gameSettings.spawnPlayer)
        //     Player.SpawnInSeconds(0);
    }

    //
    // The KeepInstancesCount component will trigger this
    // if its own instance count is zero.
    // We check to see if the object to which this component
    // is attached is an asteroid. If so, the level ends.
    //
    public void CheckLastDestroyed(KeepInstancesCount component)
    {
        if(component.gameObject.tag.Equals("Asteroid"))
        {
            CheckLevelCleared();
        }
    }

    //
    // Check the player for null.
    // If it still has lives left, increment the level number
    // and start a new one.
    void CheckLevelCleared()
    {
        CurrentLevel += 1;
        PrepareNextLevel();
    }


    //
    // Event subscriptions
    //
    void OnEnable()
    {
        KeepInstancesCount.OnLastDestroyed += CheckLastDestroyed;
    }

    void OnDisable()
    {
        KeepInstancesCount.OnLastDestroyed -= CheckLastDestroyed;
    }
}