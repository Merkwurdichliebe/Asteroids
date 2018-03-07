using UnityEngine;

/// <summary>
/// This MonoBehaviour checks collisions between objects.
/// Instead of comparing object tag strings it checks if an object
/// in on a layer which is part of a selectable layer mask.
/// </summary>

public class DestroyedByCollision : MonoBehaviour
{
    //
    // Inspector fields
    //
    public LayerMask isDestroyedBy;
    public LayerMask scoresPointsIfDestroyedBy;

    //
    // Private fields
    //
    // We need to send Kill() to all components implementing IKillable
    // (CloneWhenKilled, ExplodeWhenKilled, PlayerController)
    //
    private IKillable[] killables;
    private ScoreController scoreController;

    //
    // Initialisation 
    //
    private void Awake()
    {
        killables = gameObject.GetComponents<IKillable>();
        scoreController = GetComponent<ScoreController>();
    }

    //
    // Check for collisions.
    //
    void OnCollisionEnter2D(Collision2D collision)
    {
        // First check if the other object is on a layer
        // which can destroy this object.
        if (((1 << collision.gameObject.layer) & isDestroyedBy) != 0)
        {
            // If so, check it's on a layer that triggers point scoring
            // on this object (i.e. if this object scores points).
            // If it is, call the ScoreController method.
            if (((1 << collision.gameObject.layer) & scoresPointsIfDestroyedBy) != 0)
            {
                if (scoreController != null)
                {
                    scoreController.ScorePoints();
                }
            }

            // Trigger components which implement Kill() on this object.
            for (int i = 0; i < killables.Length; i++)
            {
                killables[i].Kill();
            }
        }
    }
}