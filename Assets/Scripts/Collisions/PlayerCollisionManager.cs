using System;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour {
    
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
            EventManager.Instance.PlayerWasHit();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        string objTag = collision.gameObject.tag;

        if (objTag == "EnemyProjectile")
        {
            Debug.Log("[PlayerCollisionManager/OnTriggerEnter2D] Player hit by enemy projectile");
            EventManager.Instance.PlayerWasHit();
        }

        if (objTag == "PowerUp")
        {
            CollectPowerUp(collision.gameObject);
        }
    }
}
