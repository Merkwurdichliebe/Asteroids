using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    // Base class for all entities (player, asteroids, UFO, powerups)
    // Member variables are declared as "protected", which makes them
    // private variables accessible by child classes.
    protected bool isAlive;
    protected Rigidbody2D rb;
    protected SpriteRenderer rend;
    protected Collider2D col;



    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        isAlive = true;
    }



    public virtual void HitByPlayer()
    {

    }



    public virtual void HitByEnemy()
    {

    }
}