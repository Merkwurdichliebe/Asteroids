using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CloneWhenKilled))]

public class ScoreControllerAsteroid : ScoreController {

    //
    // Private fields
    //

    private CloneWhenKilled ast;

    private void Awake()
    {
        ast = GetComponent<CloneWhenKilled>();
        if (ast == null)
        {
            Debug.LogError("[ScoreConstrollerAsteroid] needs a AsteroidController attached.");
        }
    }

    public override void ScorePoints()
    {
        if (ast != null && OnScorePoints != null)
        {
            // FIXME where to put this?
            int points = basePointValue * (ast.Generation + 1);
            OnScorePoints(this.gameObject, points, displayPointsLocally);
        }
    }
}