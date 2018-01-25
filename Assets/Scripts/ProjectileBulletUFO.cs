using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBulletUFO : Projectile {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity != null) entity.HitByEnemy();
        Destroy(gameObject);
    }
}