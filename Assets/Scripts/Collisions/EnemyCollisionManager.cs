using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionManager : MonoBehaviour {

    private IKillable myself;

    private void Awake()
    {
        myself = gameObject.GetComponent<IKillable>();
    }

    // Destroy this and other object if colliding with asteroid or the player.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid") ||
            collision.gameObject.CompareTag("Player"))
        {
            myself.Kill();
            collision.gameObject.GetComponent<IKillable>().Kill();
        }
    }
}