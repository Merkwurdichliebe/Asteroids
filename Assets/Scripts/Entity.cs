using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    protected bool isAlive;
    protected Rigidbody2D rb;
    protected SpriteRenderer rend;

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

/*
// Delegate type for scoring events which include a points value
public delegate void EntityDelegateScore(int points);

// We define the EventScorePoints here because
// all entities should score something
public static EntityDelegateScore EventScorePoints;

// We need to encapsulate the event call in a protected method
// so that child classes can use it
// (cf. Microsoft: How to Raise Base Class Events in Derived Classes)
protected static void OnEventScorePoints(int points)
{
    EventScorePoints(points);
}
*/