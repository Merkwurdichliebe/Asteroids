using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedByPlayerProjectiles : MonoBehaviour {

    private IKillable myself;

    private void Awake()
    {
        myself = gameObject.GetComponent<IKillable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            myself.Kill();
            ScoreController sc = GetComponent<ScoreController>();
            if (sc != null)
            {
                sc.ScorePoints();   
            }
        }
    }
}
