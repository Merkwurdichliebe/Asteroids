using UnityEngine;

[RequireComponent(typeof(ScoreManager))]

public class BonusLifeAtPoints : MonoBehaviour {

    //
    // Inspector fields 
    //
    public int bonusLifeEveryPoints;

    //
    // Private fields
    //
    private int nextBonusLifePoints;
    private ScoreManager sm;
    private GameManager gm;

    private void Awake()
    {
        sm = GetComponent<ScoreManager>();
        gm = GetComponent<GameManager>();
        nextBonusLifePoints = bonusLifeEveryPoints;
    }

    private void CheckForBonusLife()
    {
        if (sm.CurrentScore >= nextBonusLifePoints)
        {
            gm.player.Lives += 1;
            nextBonusLifePoints += bonusLifeEveryPoints;
        }
    }
}
