using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void UIDelegateMessageWithInt(int value);
    public static event UIDelegateMessageWithInt OnScoreChanged;
    public static event UIDelegateMessageWithInt OnLivesChanged;

    public delegate void UIDelegateMessageWithDuration(string text, float duration);
    public static event UIDelegateMessageWithDuration OnAnnounceMessage;

    // References to Asteroid, Player and UI script
    public GameObject pfAsteroid;
    public GameObject pfUFO;
    public PlayerController player;

    // Score, level, lives etc
    private int playerScore = 0;
    private int startingAsteroids = 4;
    public static int level;

    // Audio
    private AudioSource audioSource;

    public static bool CenterIsFree { get; private set; }

    void Awake()
    {
        // Spawn player
        player = Instantiate(player, Vector2.zero, Quaternion.identity);

        // Get reference to the AudioSource
        audioSource = GetComponent<AudioSource>();

        level = 0;

        AsteroidController.OnDestroyed += AsteroidDestroyedHandler;
        AsteroidController.OnLastAsteroidDestroyed += NextLevel;
        UFOController.OnDestroyed += UFODestroyedHandler;
        PlayerController.OnPlayerDied += PlayerDied;
        PlayerController.OnPlayerLivesZero += GameOver;

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

        OnLivesChanged(player.lives);
        NextLevel();
    }



    public void NextLevel()
    {
        // Increase level number, display it for three seconds,
        // disable (hide) the player while doing do
        CenterIsFree = true;
        level += 1;
        OnAnnounceMessage(string.Format("LEVEL {0}", level), 3.0f);
        player.gameObject.SetActive(false);
        Invoke("SpawnAsteroids", 3.0f);
        StartUFOSpawner();
    }



    void SpawnAsteroids()
    {
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

    void AsteroidDestroyedHandler(Entity obj, Transform transform, int points)
    {
        playerScore += points;
        if (OnScoreChanged != null) OnScoreChanged(playerScore);
    }

    void UFODestroyedHandler(Entity obj, Transform transform, int points)
    {
        playerScore += points;
        if (OnScoreChanged != null) OnScoreChanged(playerScore);
        StartUFOSpawner();
    }



    public void PlayerDied()
    {
        OnLivesChanged(player.lives);
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
