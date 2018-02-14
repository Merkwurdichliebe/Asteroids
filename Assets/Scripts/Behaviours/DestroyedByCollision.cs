using UnityEngine;

public class DestroyedByCollision : MonoBehaviour
{
    public LayerMask isDestroyedBy;
    public LayerMask scoresPointsIfDestroyedBy;

    // We need to send Kill() to all components implementing IKillable
    // (explode when killed, destroy when colliding...)
    private IKillable[] killables;
    private ScoreController sc;

    private void Awake()
    {
        killables = gameObject.GetComponents<IKillable>();
        sc = GetComponent<ScoreController>();
    }

    // Kill if colliding with object in selected layer
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & isDestroyedBy) != 0)
        {
            if (((1 << collision.gameObject.layer) & scoresPointsIfDestroyedBy) != 0)
            {
                if (sc != null)
                {
                    sc.ScorePoints();
                }
            }

            for (int i = 0; i < killables.Length; i++)
            {
                killables[i].Kill();
            }
        }
    }
}