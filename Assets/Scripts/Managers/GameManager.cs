﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
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
    public static event MessageEvent OnLevelStarted;

    // References to Asteroid, Player and UI script
    public int startingAsteroids;
    public GameObject PrefabAsteroid;
    public GameObject PrefabUFO;
    public PlayerController player;

    // Score, level, lives etc
    private int playerScore;
    public static int level;

    // Audio
    private AudioSource audioSource;


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
        PlayerController.OnPlayerDestroyed += HandlePlayerDestroyed;
        UFOController.OnScorePoints += HandleScorePoints;
        AsteroidController.OnScorePoints += HandleScorePoints;
    }



    void OnDisable()
    {
        AsteroidController.OnLastAsteroidDestroyed -= NextLevel;
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

        Instantiate(PrefabUFO);

        OnLivesChanged(player.livesLeft);

        // Start the first level
        NextLevel();
    }



    void NextLevel()
    {
        // Increase level number, display it for three seconds,
        // disable (hide) the player while doing do
        level += 1;
        OnCenterClear();
        OnAnnounceMessage(string.Format("LEVEL {0}", level), 3.0f);
        player.gameObject.SetActive(false);
        Invoke("SpawnAsteroids", 3.0f);
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
        if (OnLevelStarted != null) OnLevelStarted();
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




    void HandlePlayerDestroyed(int livesLeft)
    {
        OnLivesChanged(livesLeft);
        if (livesLeft == 0) {
            Destroy(player.gameObject, 3.0f);
            GameOver();
        }
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
// TODO: particle systems
// TODO: reset player acceleration sprite when respawning
// TODO: separate user input script
// TODO: fix center not clearing when ufo destroyed in it
// TODO: more asteroids sprite variations
// TODO: don't let UFO fire off screen
// TODO: enable center zone only before player respawns