using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBulletPlayer : Projectile {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player bullet collision with " + collision.gameObject.name);
        Entity entity = collision.gameObject.GetComponent<Entity>();
        entity.HitByPlayer();   
        Destroy(gameObject);
    }
}