using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerController.OnPlayerLivesZero += GameOver;
    }

    void GameOver()
    {
        Debug.Log("GameOver()");

        // Handle high score if necessary
        int highscore = PlayerPrefs.GetInt("highscore");
        int score = GetComponent<GameManager>().PlayerScore;
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
        SceneManager.LoadScene("Menu");
    } 
}
