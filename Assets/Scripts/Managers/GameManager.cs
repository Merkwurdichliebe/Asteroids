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
    [Header("Extra game prefabs")]
    public GameObject cometPrefab;
    public GameSettings gameSettings;

    //
    // Private fields
    //
    private int countAsteroids;
    private Spawner spawner;
    private MusicManager musicManager;

    // 
    // Properties
    //
    public static int CurrentLevel { get; private set; }

    //
    //  Events
    //
    public static Action OnGameLevelIntro;
    public static Action OnGameLevelStart;

    //
    // Initialisation
    //
    void Awake()
    {
        // Set the level number
        CurrentLevel = gameSettings.level;

        // Cache references
        spawner = GetComponent<Spawner>();
        musicManager = GetComponent<MusicManager>();
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
        if (OnGameLevelIntro != null) { OnGameLevelIntro(); }

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
            // CheckLevelCleared();
            StartCoroutine(LevelCleared());
        }
    }

    private IEnumerator LevelCleared()
    {
        yield return new WaitForSeconds(2);
        if (!GameOverManager.IsGameOver)
        {
            CurrentLevel += 1;
            PrepareNextLevel();
        }
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