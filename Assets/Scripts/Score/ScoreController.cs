using System;
using UnityEngine;

public class ScoreController : MonoBehaviour {

    //
    // Inspector fields
    //

    public int basePointValue;
    public int perLevelIncrease;
    public bool displayPointsLocally;

    //
    // Events
    //

    public static Action<GameObject, int, bool> OnScorePoints;

    public virtual void ScorePoints()
    {
        int points = basePointValue + perLevelIncrease * (GameManager.CurrentLevel - 1);
        if (OnScorePoints != null) { OnScorePoints(this.gameObject, points, displayPointsLocally); }
    }
}