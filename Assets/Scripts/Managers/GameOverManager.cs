using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static bool IsGameOver { get; private set; }
    public static Action OnGameOver;

    private void Awake()
    {
        IsGameOver = false;    
    }

    void GameOver()
    {
        if (OnGameOver != null) { OnGameOver(); }
        IsGameOver = true;

        // Handle high score if necessary
        int highscore = PlayerPrefs.GetInt("highscore");
        int score = GetComponent<ScoreManager>().CurrentScore;
        if ( score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
        }

        // Go back to the title screen
        StartCoroutine(DisplayMenu());
    }

    private IEnumerator DisplayMenu()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    } 

    private void OnEnable()
    {
        PlayerManager.OnPlayerLivesZero += GameOver;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerLivesZero -= GameOver;
    }
}