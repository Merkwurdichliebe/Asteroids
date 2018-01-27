using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Entity : MonoBehaviour {

    // Base class for all entities (player, asteroids, UFO, powerups)
    // Member variables are declared as "protected", which makes them
    // private variables accessible by child classes.
    protected bool isAlive;
    protected Rigidbody2D rb;
    protected SpriteRenderer rend;
    protected Collider2D col;


    // Point value of entity
    public int pointValue;

    public bool isObstacle;



    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        isAlive = true;
    }




}