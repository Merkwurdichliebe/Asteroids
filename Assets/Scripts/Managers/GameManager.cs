using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    // Event Messaging
    public static event Action<int> OnScoreChanged;
    public static event Action<string, float> OnAnnounceMessage;

    public delegate void MessageEvent();
    public static event MessageEvent OnLevelStarted;
    public static event MessageEvent OnGameOver;

    // References to Asteroid, Player and UI script
    public int startingAsteroids;
    public GameObject PrefabAsteroid;
    public GameObject PrefabUFO;
    public GameObject PrefabPowerup;
    public PlayerController player;

    // Score, level, lives etc
    private int playerScore;

    public int PlayerScore
    {
        get
        {
            return playerScore;
        }
    }

    public static int level;

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

        // Reset player to center and enable (unhide) it
        player.gameObject.transform.position = Vector2.zero;
        player.gameObject.SetActive(true);
        if (OnLevelStarted != null) OnLevelStarted();
    }



    void HandleScorePoints(Entity e)
    {
        playerScore += e.pointValue;
        if (OnScoreChanged != null) OnScoreChanged(playerScore);
    }
}

// TODO: powerups
// TODO: end level only when UFO not here
// TODO: reset player acceleration sprite when respawning
// TODO: separate user input script
// TODO: more asteroids sprite variations
