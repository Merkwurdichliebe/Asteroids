using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static Action OnGameOver;

    void GameOver()
    {
        // Debug.Log("[GameOverManager/GameOver]");

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

    IEnumerator DisplayMenu()
    {
        yield return new WaitForSeconds(5);
        StopAllCoroutines();
        if (OnGameOver != null) { OnGameOver(); }
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