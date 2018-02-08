using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedByProjectile : MonoBehaviour
{

    private IKillable myself;
    public enum ProjectileType { PlayerProjectile, EnemyProjectile }

    public ProjectileType projectileType;
    public bool scorePoints;

    private void Awake()
    {
        myself = gameObject.GetComponent<IKillable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(projectileType.ToString()))
        {
            myself.Kill();
            ScoreController sc = GetComponent<ScoreController>();
            if (sc != null && scorePoints)
            {
                sc.ScorePoints();
            }
        }
    }
}