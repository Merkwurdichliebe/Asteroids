using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    //
    // Private fields
    //
    private GameManager gm;
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
        gm = GetComponent<GameManager>();
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
        if (gm.gameSettings.bonusLifeEveryPoints > 0)
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
        if (CurrentScore >= lastBonusLifePoints + gm.gameSettings.bonusLifeEveryPoints)
        {
            gm.Player.Lives += 1;
            lastBonusLifePoints += gm.gameSettings.bonusLifeEveryPoints;
        }
    }
}