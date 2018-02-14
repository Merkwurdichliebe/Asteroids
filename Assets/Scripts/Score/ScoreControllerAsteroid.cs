using UnityEngine;

[RequireComponent(typeof(CloneWhenKilled))]

public class ScoreControllerAsteroid : ScoreController {

    //
    // Private fields
    //
    private CloneWhenKilled clone;

    private void Awake()
    {
        clone = GetComponent<CloneWhenKilled>();
        if (clone == null)
        {
            Debug.LogError("[ScoreConstrollerAsteroid] needs a CloneWhenKilled attached.");
        }
    }

    public override void ScorePoints()
    {
        if (clone != null && OnScorePoints != null)
        {
            int points = basePointValue * (clone.Generation + 1);
            OnScorePoints(this.gameObject, points, displayPointsLocally);
        }
    }
}