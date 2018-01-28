using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType { FrontAndBack, Whatever }

public class PowerUpController : Entity {

    public PowerUpType powerUpType;

    private void Start()
    {
        MoveToCenter();
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }



    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
