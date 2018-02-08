using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCollisionManager : MonoBehaviour
{

    private IKillable myself;

    private void Awake()
    {
        myself = gameObject.GetComponent<IKillable>();
    }

    // Destroy this and other object if colliding with enemy or the player.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Player"))
        {
            myself.Kill();
        }
    }
}