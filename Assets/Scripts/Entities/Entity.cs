using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public abstract class Entity : MonoBehaviour {
    
    // Base class for all entities (player, asteroids, UFO, powerups)
    // Member variables are declared as "protected", which makes them
    // private variables accessible by child classes.
    protected bool isAlive;
    protected Rigidbody2D rb;
    protected SpriteRenderer rend;
    protected Collider2D col;

    // On Awake we grab references for the required components
    // and set isAlive to true.
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        // SetAlive(true);
        // isAlive = true; // FIXME should call SetAlive but Null reference ???
    }


    /// <summary>
    /// Toggles state of isAlive member variable.
    /// Toggles active state of Renderer, Collider and Rigibody.
    /// We need this in order to make objects disappear and not respond
    /// to collisions when we need to avoid calling Destroy().
    /// </summary>
    /// 
    public virtual void SetAlive(bool active) // FIXME try to make this an inherited property
    {
        isAlive = active;
        rend.enabled = active;
        col.enabled = active;
        rb.isKinematic = !active;
        Debug.Log("[Entity/SetAlive] " + gameObject.name + " : " + active);
    }

}