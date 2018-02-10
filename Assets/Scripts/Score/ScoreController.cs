using System;
using UnityEngine;

public class ScoreController : MonoBehaviour {

    //
    // Inspector fields
    //

    public int basePointValue;
    public int perLevelIncrease;
    public bool displayPointsWhenKilled;

    //
    // Events
    //

    public static Action<GameObject, int, bool> OnScorePoints;

    public virtual void ScorePoints()
    {
        int points = basePointValue + perLevelIncrease * (GameManager.CurrentLevel);
        if (OnScorePoints != null) { OnScorePoints(this.gameObject, points, displayPointsWhenKilled); }
    }
}