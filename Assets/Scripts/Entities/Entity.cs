using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

// Base class for all entities (player, asteroids, UFO, powerups)
// that use physics and the SpriteRenderer.
// Member variables are declared as "protected", which makes them
// private variables accessible by child classes.

public abstract class Entity : MonoBehaviour {
    
    protected Rigidbody2D rb;
    protected SpriteRenderer rend;
    protected Collider2D col;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }
}