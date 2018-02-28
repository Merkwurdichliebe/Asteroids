using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    //
    // Private fields
    //
    private GameManager gameManager;
    private PlayerManager playerManager;
    private UIManager ui;
    private int lastBonusLifePoints;

    //
    // Properties
    //
    public int CurrentScore { get; private set; }

    //
    // Initialisation 
    //
    private void Awake()
    {
        ui = GetComponent<UIManager>();
        playerManager = GetComponent<PlayerManager>();
        gameManager = GetComponent<GameManager>();
    }

    //
    // Subscribe to ScorePoints events 
    //
    private void OnEnable()
    {
        ScoreController.OnScorePoints += ScorePoints;
    }

    private void OnDisable()
    {
        ScoreController.OnScorePoints -= ScorePoints;
    }

    //
    // Update the score 
    //
    public void ScorePoints(GameObject obj, int points, bool displayPointsLocally)
    {
        CurrentScore += points;
        if (displayPointsLocally)
        {
            ui.ShowTextAtScreenPosition(obj, points.ToString());
        }
        if (gameManager.gameSettings.bonusLifeEveryPoints > 0)
        {
            CheckForBonusLife();
        }
        ui.UpdateScore(CurrentScore);
    }

    //
    // Check if CurrentScore is larger than the bonus life threshold
    //
    private void CheckForBonusLife()
    {
        if (CurrentScore >= lastBonusLifePoints + gameManager.gameSettings.bonusLifeEveryPoints)
        {
            playerManager.Player.Lives += 1;
            lastBonusLifePoints += gameManager.gameSettings.bonusLifeEveryPoints;
        }
    }
}