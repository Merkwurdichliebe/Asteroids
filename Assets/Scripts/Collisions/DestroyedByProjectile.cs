using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedByProjectile : MonoBehaviour
{


    public enum ProjectileType { PlayerProjectile, EnemyProjectile }

    public ProjectileType projectileType;
    private ScoreController sc;
    private IKillable[] killables;

    //
    // Get references to the IKillable interface
    // and to the ScoreController component.
    // If there is no ScoreController component present,
    // destruction will not score points.
    //
    private void Awake()
    {
        killables = gameObject.GetComponents<IKillable>();
        sc = GetComponent<ScoreController>();
    }

    //
    // Kill this object when it collides with this projectile type.
    // Score points if there is a ScoreController present.
    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(projectileType.ToString()))
        {
            foreach (IKillable killable in killables)
            {
                killable.Kill();
            }
            if (sc != null)
            {
                sc.ScorePoints();
            }
        }
    }
}