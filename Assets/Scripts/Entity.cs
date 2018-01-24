using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    protected bool isAlive;
    protected Rigidbody2D rb;
    protected SpriteRenderer rend;
    protected GameManager gameManager;

    public virtual void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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
