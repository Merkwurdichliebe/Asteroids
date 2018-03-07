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
    public GameObject spawnerPrefab;

    //
    // Private fields
    //
    private GameObject spawner;

    // 
    // Properties
    //
    public static int CurrentLevel { get; private set; }

    //
    //  Events
    //
    public static Action OnGameLevelIntro;
    public static Action OnGameLevelStart;
    public static Action OnGameLevelComplete;

    //
    // Initialisation
    //
    void Awake()
    {
        // Set the level number
        CurrentLevel = gameSettings.level;
    }

    //
    // Start the game sequence
    //
    void Start()
    {
        if (gameSettings.spawnOthers)
            spawner = Instantiate(spawnerPrefab);

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
            StartCoroutine(PlayLevelIntroSequence());    
        else
            StartNextLevel();
    }

    //
    // Level intro sequence
    //
    IEnumerator PlayLevelIntroSequence()
    {
        // Send message
        if (OnGameLevelIntro != null) { OnGameLevelIntro(); }

        // Disable the Spawner
        if (spawner)
            spawner.SetActive(false);

        // Animate the comet effect
        // Particle System delay is set to 0,05 to avoid emission at (0,0) on first frame
        Instantiate(cometPrefab);
        
        // Wait for 3 seconds and start the level
        yield return new WaitForSeconds(3);
        StartNextLevel();
    }

    //
    // Start the level
    //
    void StartNextLevel()
    {
        // Must be done first for the spawner to receive the message!
        if (spawner)
            spawner.SetActive(true);

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
        if(component.gameObject.CompareTag("Asteroid"))
        {
            // Send message
            if (OnGameLevelComplete != null) { OnGameLevelComplete(); }
            StartCoroutine(LevelCleared());
        }
    }

    private IEnumerator LevelCleared()
    {
        yield return new WaitForSeconds(1);
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

// TODO: Increase spawner frequency with level
// TODO: Allow more simultaneous ufos per level - Spawnable class should contain settings for itself and these could be overriden in Spawner for testing
// TODO: fix UFO shooting twice sometimes
// TODO: try lerping particle system rotation with shield
// TODO: work on pickup FX
// TODO: implement command & state machine, make UFO search with OverlapSphere : https://www.youtube.com/watch?v=D6hAftj3AgM
// TODO: show lives left as icons
// TODO: Make player die reset bomb bays and weapons in the same place (now they are in two classes)