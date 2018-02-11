using UnityEngine;

[RequireComponent(typeof(CloneWhenKilled))]

public class ScoreControllerAsteroid : ScoreController {

    //
    // Private fields
    //
    private CloneWhenKilled cloneBehaviour;

    private void Awake()
    {
        cloneBehaviour = GetComponent<CloneWhenKilled>();
        if (cloneBehaviour == null)
        {
            Debug.LogError("[ScoreConstrollerAsteroid] needs a CloneWhenKilled attached.");
        }
    }

    public override void ScorePoints()
    {
        if (cloneBehaviour != null && OnScorePoints != null)
        {
            int points = basePointValue * (cloneBehaviour.Generation + 1);
            OnScorePoints(this.gameObject, points, displayPointsLocally);
        }
    }
}