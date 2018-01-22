using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Entities Prefabs
    public GameObject pfAsteroid;

    // Reference to other scripts
    public PlayerController player;
    private UIManager UIManager;

    // Center trigger
    private Collider2D centerCollider;
    public bool centerIsFree = true;

    // Score, level, lives etc
    private int playerScore = 0;
    private int startingAsteroids = 4;
    public int level = 1;

    // Audio
    private AudioSource audioSource;


    void Awake()
    {
        // Get reference to the UI Manager script
        UIManager = gameObject.GetComponent<UIManager>();

        // Spawn player
        player = Instantiate(player, Vector2.zero, Quaternion.identity);

        audioSource = GetComponent<AudioSource>();
    }



    void Start()
    {
        AsteroidController.countAsteroids = 0;
        UIManager.UpdateLives(player.lives);
        StartLevel();
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        centerIsFree = true;
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        centerIsFree = false;
    }



    void StartLevel()
    {
        UIManager.Announce(string.Format("LEVEL {0}", level));
        player.gameObject.SetActive(false);
        Invoke("SpawnAsteroids", 3.0f);
    }



    public void LevelDone()
    {
        level += 1;
        StartLevel();
    }



    void SpawnAsteroids()
    {
        UIManager.Announce("");
        // Spawn starting asteroids
        for (int i = 0; i < startingAsteroids + level - 1; i++)
        {
            Instantiate(pfAsteroid, Vector2.zero, Quaternion.identity);
        }

        player.gameObject.transform.position = Vector2.zero;
        player.gameObject.SetActive(true);
    }



    public void GameOver()
    {
        UIManager.Announce("GAME OVER");
        int highscore = PlayerPrefs.GetInt("highscore");
        if (playerScore > highscore)
        {
            PlayerPrefs.SetInt("highscore", playerScore);
        }
        Invoke("DisplayMenu", 6.0f);
    }



    void DisplayMenu()
    {
        SceneManager.LoadScene("Menu");
    } 



    public void ScorePoints(int points)
    {
        playerScore += points;
        UIManager.UpdateScore(playerScore);
    }



    public void PlayerDied()
    {
        UIManager.UpdateLives(player.lives);
    }

}


// TODO: ufo
// TODO: powerups
