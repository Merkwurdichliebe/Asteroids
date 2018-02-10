using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public int bonusLifeEveryPoints;

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
    public void ScorePoints(GameObject obj, int points, bool displayPointsWhenKilled)
    {
        CurrentScore += points;
        if (displayPointsWhenKilled)
        {
            ui.ShowPointsAtScreenPosition(obj, points);
        }
        if (bonusLifeEveryPoints > 0)
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
        if (CurrentScore >= lastBonusLifePoints + bonusLifeEveryPoints)
        {
            gm.player.Lives += 1;
            lastBonusLifePoints += bonusLifeEveryPoints;
        }
    }
}