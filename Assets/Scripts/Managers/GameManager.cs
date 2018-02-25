﻿using System;
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
    private AsteroidSpawner asteroidSpawner;

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
    public static Action OnGameLevelReady;
    public static Action OnGameLevelStart;

    //
    // Initialisation
    //
    void Awake()
    {
        // Check for unconnected prefabs
        Assert.IsNotNull(playerPrefab);
        Assert.IsNotNull(spawnSafeZonePrefab);
        
        // Set the level number
        CurrentLevel = startWithLevel;

        // Create the Player spawn safe zone
        spawnSafeZone = Instantiate(spawnSafeZonePrefab);

        // Get a reference to the spawners
        spawner = GetComponent<Spawner>();
        asteroidSpawner = GetComponent<AsteroidSpawner>();

        // Spawn the player
        if (spawnPlayer)
        {
            Player = Instantiate(playerPrefab);
            Player.Lives = startWithPlayerLives;
        }   
    }

    //
    // Start the game sequence
    //
    void Start()
    {
        PrepareNextLevel();
    }

    //
    // Do the level intro sequence if needed,
    // otherwise start the level directly.
    //
    void PrepareNextLevel()
    {
        StopAllCoroutines();

        if (playLevelIntro)
            StartCoroutine(ReadyNextLevel());    
        else
            StartNextLevel();
    }

    // FIXME: go to next level when asteroids zero even when no player
    // Level into sequence
    //
    IEnumerator ReadyNextLevel()
    {
        // Send message
        if (OnGameLevelReady != null) { OnGameLevelReady(); }

        // Deactivate the player & safe zone
        if (Player != null) 
            Player.ActiveInScene = false;

        // This needs to come after player deactivation or it will reactivate
        spawnSafeZone.SetActive(false);

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

        // Activate the safe zone
        spawnSafeZone.SetActive(true);
        
        // Spawn asteroids
        if (spawnAsteroids)
            asteroidSpawner.Spawn(startWithAsteroids + CurrentLevel - 1);
    
        // Spawn the player
        if (spawnPlayer)
            Player.SpawnInSeconds(0);
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

    void CheckLevelCleared()
    {
        if (Player != null)
        {
            if (Player.Lives != 0)
            {
                CurrentLevel += 1;
                PrepareNextLevel();
            }
        }
    }

    //
    // These are called when the different "OnPlayer" events
    // subscribed to in OnEnable() are received.
    // We use this to enable and disable the safe zone
    // depending on whether the player has spawned
    // or despawned.
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
    // Event subscriptions
    //
    void OnEnable()
    {
        KeepInstancesCount.OnLastDestroyed += CheckLastDestroyed;
        PlayerController.OnPlayerSpawned += DisableSafeZone;
        PlayerController.OnPlayerDestroyed += EnableSafeZone;
        PlayerController.OnPlayerDespawned += EnableSafeZone;
    }

    void OnDisable()
    {
        KeepInstancesCount.OnLastDestroyed -= CheckLastDestroyed;
        PlayerController.OnPlayerSpawned -= DisableSafeZone;
        PlayerController.OnPlayerDestroyed -= EnableSafeZone;
        PlayerController.OnPlayerDespawned -= EnableSafeZone;
    }
}