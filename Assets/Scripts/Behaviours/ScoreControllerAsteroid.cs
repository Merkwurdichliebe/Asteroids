using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AsteroidController))]

public class ScoreControllerAsteroid : ScoreController {

    //
    // Private fields
    //

    private AsteroidController ast;

    private void Awake()
    {
        ast = GetComponent<AsteroidController>();
        if (ast == null)
        {
            Debug.LogError("[ScoreConstrollerAsteroid] needs a AsteroidController attached.");
        }
    }

    public override void ScorePoints()
    {
        if (ast != null && OnScorePoints != null)
        {
            int points = basePointValue * (ast.Phase + 1);
            OnScorePoints(this.gameObject, points, displayPointsWhenKilled);
        }
    }
}