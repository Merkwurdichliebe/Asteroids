using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject pfAsteroid;
    public GameObject pfPlayer;

    public GameObject player;
    public PlayerController playerController;

    private UIManager UIManager;

    private int playerScore = 0;
    private int playerLives = 3;
    private bool gameover = false;
    private float playerDiedTime;



    void Awake()
    {
        // Get reference to the UI Manager script
        UIManager = gameObject.GetComponent<UIManager>();

        // Spawn player
        // Needs to be done in Awake so that the UI Manager Script
        // can get the reference to the player in Start
        player = SpawnPlayer(Vector2.zero);
        playerController = player.GetComponent<PlayerController>();
    }



    void Start()
    {
        // Spawn asteroids
        for (int i = 0; i < 4; i++)
        {
            SpawnAsteroid(new Vector2(Random.Range(-15, 15), Random.Range(3, 6)), 1);
        }

        UIManager.UpdateScore(playerScore);
        UIManager.UpdateLives(playerLives);
    }



    void Update()
    {
        if (!playerController.isAlive && (Time.time - playerDiedTime > 3) && !gameover)
        {
            playerController.Respawn();
        }

        if (gameover)
        {
            StartCoroutine(GameOver());
        }
    }



    IEnumerator GameOver()
    {
        UIManager.DisplayGameOver();
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene("Menu");
    }



    public void HitAsteroid(GameObject go)
    {
        // Get the phase of the destroyed asteroid
        // Spawn two smaller ones if needed
        int phase = go.GetComponent<AsteroidController>().Phase;
        if (phase < 3)
        {
            SpawnAsteroid(go.transform.position, phase + 1);
            SpawnAsteroid(go.transform.position, phase + 1);
        }
        Destroy(go);
        ScorePoints(phase);
    }



    private void SpawnAsteroid(Vector2 pos, int phase)
    {
        // Instantiate the asteroid and set its phase
        GameObject ast = Instantiate(pfAsteroid, Vector2.zero, Quaternion.identity);
        AsteroidController astController = ast.GetComponent(typeof(AsteroidController)) as AsteroidController;
        astController.Phase = phase;
        ast.transform.position = pos;
    }



    private GameObject SpawnPlayer(Vector2 pos)
    {
        return Instantiate(pfPlayer, pos, Quaternion.identity);
    }



    void ScorePoints(int points)
    {
        playerScore += points;
        UIManager.UpdateScore(playerScore);
    }



    public void PlayerDied(GameObject go)
    {
        playerLives -= 1;
        UIManager.UpdateLives(playerLives);
        playerDiedTime = Time.time;
        if (playerLives == 0)
        {
            gameover = true;
        }
    }

}