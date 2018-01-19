using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Entities Prefabs
    public GameObject pfAsteroid;
    // public GameObject pfPlayer;

    // Reference to other scripts
    public PlayerController player;
    private UIManager UIManager;

    // Score, level, lives etc
    private int playerScore = 0;
    private int playerLives = 3;
    private bool gameover = false;
    private float playerDiedTime;
    private int startingAsteroids = 4;
    private int level = 1;

    // Audio
    private AudioSource audioSource;
    public AudioClip soundPlayerDestroyed;


    void Awake()
    {
        // Get reference to the UI Manager script
        UIManager = gameObject.GetComponent<UIManager>();

        // Spawn player
        // Needs to be done in Awake so that the UI Manager Script
        // can get the reference to the player in Start
        player = Instantiate(player, Vector2.zero, Quaternion.identity);
        player.isAlive = false;

        audioSource = GetComponent<AudioSource>();
    }



    void Start()
    {
        // Spawn starting asteroids
        for (int i = 0; i < startingAsteroids; i++)
        {
            Instantiate(pfAsteroid, Vector2.zero, Quaternion.identity);
        }

        player.isAlive = true;

        UIManager.UpdateScore(playerScore);
        UIManager.UpdateLives(playerLives);
    }



    void Update()
    {
        if (!player.isAlive && (Time.time - playerDiedTime > 3) && !gameover)
        {
            player.Respawn();
        }

        if (gameover)
        {
            StartCoroutine(GameOver());
        }

        if (AsteroidController.countAsteroids == 0) {
            Debug.Log("Level Done");
        }
    }



    IEnumerator GameOver()
    {
        UIManager.DisplayGameOver();
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene("Menu");
    }



    public void ScorePoints(int points)
    {
        playerScore += points;
        UIManager.UpdateScore(playerScore);
    }



    public void PlayerDied(GameObject go)
    {
        audioSource.PlayOneShot(soundPlayerDestroyed);
        playerLives -= 1;
        UIManager.UpdateLives(playerLives);
        playerDiedTime = Time.time;
        if (playerLives == 0)
        {
            gameover = true;
        }
    }

}