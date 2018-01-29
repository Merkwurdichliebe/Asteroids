using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Messaging delegates
    // -------------------------------------------------------------------------

    public static Action<int> OnScoreChanged;
    public static Action<string, float> OnAnnounceMessage;
    public static Action OnLevelStarted;

    // -------------------------------------------------------------------------
    // Inspector variables
    // -------------------------------------------------------------------------

    public int startingAsteroids;
    public static int level;
    public GameObject PrefabAsteroid;
    public GameObject PrefabUFO;
    public GameObject PrefabPowerup;
    public PlayerController player;

    // -------------------------------------------------------------------------
    // Private variables and properties
    // -------------------------------------------------------------------------

    private int playerScore;
    public int PlayerScore { get { return playerScore; } }

    // -------------------------------------------------------------------------
    // Setup methods
    // -------------------------------------------------------------------------

    void Awake()
    {
        Assert.IsNotNull(PrefabAsteroid);
        Assert.IsNotNull(PrefabUFO);
        Assert.IsNotNull(player);

        // Reset the (static) count variable
        AsteroidController.countAsteroids = 0;

        level = 0;
    }

    void OnEnable()
    {
        AsteroidController.OnLastAsteroidDestroyed += NextLevel;
        UFOController.OnScorePoints += HandleScorePoints;
        AsteroidController.OnScorePoints += HandleScorePoints;
    }

    void OnDisable()
    {
        AsteroidController.OnLastAsteroidDestroyed -= NextLevel;
        UFOController.OnScorePoints -= HandleScorePoints;
        AsteroidController.OnScorePoints -= HandleScorePoints;
    }

    void Start()
    {
        // Spawn player
        player = Instantiate(player, Vector2.zero, Quaternion.identity);
        player.Lives = 3;

        // Spawn UFO
        Instantiate(PrefabUFO);

        // Start the first level
        NextLevel();
    }



    void NextLevel()
    {
        // Increase level number, display it for three seconds,
        // disable (hide) the player while doing do
        level += 1;
        Debug.Log("Level " + level + " starting");
        OnAnnounceMessage(string.Format("LEVEL {0}", level), 3.0f);
        player.gameObject.SetActive(false);
        StartCoroutine(SpawnAsteroids());
    }



    IEnumerator SpawnAsteroids()
    {
        yield return new WaitForSeconds(3.0f);
        // Spawn asteroids based on level number
        Assert.IsNotNull(PrefabAsteroid);
        for (int i = 0; i < startingAsteroids + level - 1; i++)
        {
            Instantiate(PrefabAsteroid, Vector2.zero, Quaternion.identity);
        }
        Debug.Log("Asteroids spawned");
        player.gameObject.SetActive(true); // FIXME find a clean solution for spawning
        if (OnLevelStarted != null) OnLevelStarted();
    }



    void HandleScorePoints(Entity e)
    {
        playerScore += e.pointValue;
        if (OnScoreChanged != null) OnScoreChanged(playerScore);
    }
}

// TODO: powerups
// TODO: clean player spawning routine
// TODO: fix screen aspect and wrap around issues
// TODO: end level only when UFO not here
// TODO: reset player acceleration sprite when respawning
// TODO: more asteroids sprite variations
