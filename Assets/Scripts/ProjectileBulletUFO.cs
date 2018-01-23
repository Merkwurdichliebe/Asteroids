using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBulletUFO : Projectile {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("UFO bullet collision with " + collision.gameObject.name);
        Entity entity = collision.gameObject.GetComponent<Entity>();
        entity.HitByEnemy();   
        Destroy(gameObject);
    }
}