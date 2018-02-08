using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public int CurrentScore { get; private set; }

    //
    // Private fields
    //

    private UIManager ui;

    private void Awake()
    {
        ui = GetComponent<UIManager>();
    }

    private void OnEnable()
    {
        ScoreController.OnScorePoints += ScorePoints;
    }

    private void OnDisable()
    {
        ScoreController.OnScorePoints -= ScorePoints;
    }

    public void ScorePoints(GameObject obj, int points, bool displayPointsWhenKilled)
    {
        CurrentScore += points;
        if (displayPointsWhenKilled)
        {
            ui.ShowPointsAtScreenPosition(obj, points);
        }
        ui.UpdateScore(CurrentScore);
    }
}