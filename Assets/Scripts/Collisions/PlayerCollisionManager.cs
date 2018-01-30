using System;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour {

    public static Action OnPlayerWasHit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        string objTag = collision.gameObject.tag;

        if (objTag == "EnemyProjectile")
        {
            collision.gameObject.SetActive(false);
            Debug.Log("[PlayerCollisionManager/OnTriggerEnter2D] Player hit by enemy projectile");
            OnPlayerWasHit();
        }

        if (objTag == "PowerUp")
        {
            CollectPowerUp(collision.gameObject);
        }
    }



    private void CollectPowerUp(GameObject obj)
    {
        PowerUpController pu = obj.GetComponent<PowerUpController>();
        if (pu.powerUpType == PowerUpType.FrontAndBack)
        {
            Debug.Log("[PlayerCollisionManager/OnTriggerEnter2D] FrontAndBack Collected");
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        string goTag = collision.gameObject.tag;

        if (goTag == "Asteroid" || goTag == "Enemy")
        {
            Debug.Log("[PlayerCollisionManager/OnTriggerEnter2D] Player collided with " + goTag);
            OnPlayerWasHit();
        }
    }
}
