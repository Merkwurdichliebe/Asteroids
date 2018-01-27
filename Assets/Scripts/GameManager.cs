using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // Event Messaging
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnLivesChanged;
    public static event Action<string, float> OnAnnounceMessage;

    public delegate void MessageEvent();
    public static event MessageEvent OnCenterClear;
    public static event MessageEvent OnCenterOccupied;

    // References to Asteroid, Player and UI script
    public GameObject PrefabAsteroid;
    public GameObject PrefabUFO;
    public PlayerController player;

    // Score, level, lives etc
    private int playerScore;
    private int startingAsteroids = 4;
    public static int level;

    // Audio
    private AudioSource audioSource;

    // For tuning
    public float UFOSpawnFrequency;
    public float UFOSpawnProbability;





    void Awake()
    {
        Assert.IsNotNull(PrefabAsteroid);
        Assert.IsNotNull(PrefabUFO);
        Assert.IsNotNull(player);

        // Get reference to the GameManager AudioSource
        audioSource = GetComponent<AudioSource>();

        // Reset the (static) count variable
        AsteroidController.countAsteroids = 0;

        level = 0;
    }



    void OnEnable()
    {
        AsteroidController.OnLastAsteroidDestroyed += NextLevel;
        UFOController.OnUFODestroyed += HandleUFODestroyed;
        PlayerController.OnPlayerDestroyed += HandlePlayerDestroyed;
        UFOController.OnScorePoints += HandleScorePoints;
        AsteroidController.OnScorePoints += HandleScorePoints;
    }



    void OnDisable()
    {
        AsteroidController.OnLastAsteroidDestroyed -= NextLevel;
        UFOController.OnUFODestroyed -= HandleUFODestroyed;
        PlayerController.OnPlayerDestroyed -= HandlePlayerDestroyed;
        UFOController.OnScorePoints -= HandleScorePoints;
        AsteroidController.OnScorePoints -= HandleScorePoints;
    }



    void Start()
    {
        // Play background music
        audioSource.Play();

        // Spawn player
        player = Instantiate(player, Vector2.zero, Quaternion.identity);

        OnLivesChanged(player.livesLeft);

        // Start the first level
        NextLevel();
    }



    void NextLevel()
    {
        // Increase level number, display it for three seconds,
        // disable (hide) the player while doing do
        OnCenterClear();
        level += 1;
        OnAnnounceMessage(string.Format("LEVEL {0}", level), 3.0f);
        player.gameObject.SetActive(false);
        Invoke("SpawnAsteroids", 3.0f);
        StartUFOSpawner();
    }



    void SpawnAsteroids()
    {
        // Spawn asteroids based on level number
        Assert.IsNotNull(PrefabAsteroid);
        for (int i = 0; i < startingAsteroids + level - 1; i++)
        {
            Instantiate(PrefabAsteroid, Vector2.zero, Quaternion.identity);
        }

        // Reset player to center and enable (unhide) it
        player.gameObject.transform.position = Vector2.zero;
        player.gameObject.SetActive(true);
    }



    void StartUFOSpawner()
    {
        InvokeRepeating("SpawnUFO", 3.0f, UFOSpawnFrequency);
    }



    void SpawnUFO()
    {
        if (Random.value < UFOSpawnProbability)
        {
            Instantiate(PrefabUFO);
            CancelInvoke("SpawnUFO");
        }
    }



    void GameOver()
    {
        // Display Game Over message
        if (OnAnnounceMessage != null) OnAnnounceMessage("GAME OVER", 6.0f);

        // Handle high score if necessary
        int highscore = PlayerPrefs.GetInt("highscore");
        if (playerScore > highscore)
        {
            PlayerPrefs.SetInt("highscore", playerScore);
        }

        // Go back to the title screen
        Invoke("DisplayMenu", 6.0f);
    }



    void DisplayMenu()
    {
        SceneManager.LoadScene("Menu");
    } 



    void HandleScorePoints(Entity e)
    {
        playerScore += e.pointValue;
        if (OnScoreChanged != null) OnScoreChanged(playerScore);
    }



    void HandleUFODestroyed()
    {
        StartUFOSpawner();
    }



    void HandlePlayerDestroyed(int livesLeft)
    {
        OnLivesChanged(livesLeft);
        if (livesLeft == 0) GameOver();
    }



    // Methods for checking if the center of the screen
    // is free from danger, before respawning the player

    void OnTriggerExit2D(Collider2D collision)
    {
        OnCenterClear();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        OnCenterOccupied();
    }

}

// TODO: powerups
// TODO: end level only when UFO not here
// TODO: UFO death animation