using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // References to Asteroid, Player and UI script
    public GameObject pfAsteroid;
    public GameObject pfUFO;
    public PlayerController player;
    private UIManager UIManager;

    // Center trigger collider, for avoiding player spawning on asteroids
    private Collider2D centerCollider;

    // Score, level, lives etc
    private int playerScore = 0;
    private int startingAsteroids = 4;
    public static int level;

    // Audio
    private AudioSource audioSource;

    public static bool CenterIsFree { get; private set; }

    void Awake()
    {
        // Get reference to the UI Manager script
        UIManager = gameObject.GetComponent<UIManager>();

        // Spawn player
        player = Instantiate(player, Vector2.zero, Quaternion.identity);

        // Get reference to the AudioSource
        audioSource = GetComponent<AudioSource>();

        level = 0;

        // Time.timeScale = 0.5f;

        // Subscribe to event delegates
        EventManager.OnScorePoints += ScorePoints;
        EventManager.OnPlayerDestroyed += PlayerDied;
        EventManager.OnLivesEqualsZero += GameOver;
        EventManager.OnLastAsteroidDestroyed += NextLevel;
        EventManager.OnUFODestroyed += StartUFOSpawner;
    }



    void StartUFOSpawner()
    {
        InvokeRepeating("SpawnUFO", 3.0f, 9.0f);
    }



    void SpawnUFO()
    {
        if (Random.value < 0.5)
        {
            Instantiate(pfUFO);
            CancelInvoke("SpawnUFO");
        }
    }



    void Start()
    {
        // Reset the (static) count variable
        AsteroidController.countAsteroids = 0;

        // Play background music
        audioSource.Play();

        UIManager.UpdateLives(player.lives);
        NextLevel();
    }



    public void NextLevel()
    {
        // Increase level number, display it for three seconds,
        // disable (hide) the player while doing do
        CenterIsFree = true;
        level += 1;
        UIManager.Announce(string.Format("LEVEL {0}", level));
        player.gameObject.SetActive(false);
        Invoke("SpawnAsteroids", 3.0f);
        StartUFOSpawner();
    }



    void SpawnAsteroids()
    {
        // Clear the level message
        UIManager.Announce("");

        // Spawn asteroids based on level number
        for (int i = 0; i < startingAsteroids + level - 1; i++)
        {
            Instantiate(pfAsteroid, Vector2.zero, Quaternion.identity);
        }

        // Reset player to center and enable (unhide) it
        player.gameObject.transform.position = Vector2.zero;
        player.gameObject.SetActive(true);
    }



    public void GameOver()
    {
        // Display Game Over message
        UIManager.Announce("GAME OVER");

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



    public void ScorePoints(int points)
    {
        playerScore += points;
        EventManager.UIUpdateScore(playerScore);
    }



    public void PlayerDied()
    {
        EventManager.UIUpdateLives(player.lives);
    }



    // Methods for checking if the center of the screen
    // is free from asteroids, before respawning the player

    private void OnTriggerExit2D(Collider2D collision)
    {
        CenterIsFree = true;
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        CenterIsFree = false;
    }

}


// TODO: powerups
