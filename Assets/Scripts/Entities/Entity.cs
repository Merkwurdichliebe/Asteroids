using UnityEngine;

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

    // Point value of entity for scoring
    public int pointValue;



    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        isAlive = true;
    }



    public virtual void SetActive(bool active)
    {
        isAlive = active;
        rend.enabled = active;
        col.enabled = active;
        rb.isKinematic = !active;
    }
}