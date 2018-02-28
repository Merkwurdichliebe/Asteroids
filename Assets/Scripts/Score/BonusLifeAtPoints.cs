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
    private ScoreManager scoreManager;
    private PlayerManager playerManager;

    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
        playerManager = GetComponent<PlayerManager>();
        nextBonusLifePoints = bonusLifeEveryPoints;
    }

    private void CheckForBonusLife()
    {
        if (scoreManager.CurrentScore >= nextBonusLifePoints)
        {
            playerManager.Player.Lives += 1;
            nextBonusLifePoints += bonusLifeEveryPoints;
        }
    }
}