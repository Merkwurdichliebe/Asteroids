using UnityEngine;
using System.Collections;

// The Projectile class is designed to be instantiated as part of
// an object pool. It should be disabled rather than destroyed.

public class Projectile : MonoBehaviour {

    //
    // Inspector fields 
    //
    [Header("Speed & Lifespan can be overriden by FireProjectile")]
    public float speed;
    public float lifespan;
    public bool explodeOnHit;
    public GameObject explosion;

    //
    // Private fields 
    //
    private Rigidbody2D rb;
    private bool shouldExplode;
    private float timeAtStart;

    //
    // Initialisation 
    //
    void Awake()
    {
        rb = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
        if (explodeOnHit && explosion == null)
        {
            Debug.LogError("[Projectile/Awake] Explode selected but none is assigned.");
        }
    }

    //
    // Launch the projectile.
    // We do this in OnEnable because projectiles are pooled.
    //
    void OnEnable()
    {
        timeAtStart = Time.time;
        rb.AddRelativeForce(Vector2.up * speed / 10, ForceMode2D.Impulse);
    }

    //
    // Deactivate the projectile when its lifespan has been reached.
    //
    private void Update()
    {
        if (Time.time - timeAtStart > lifespan)
        {
            Destroy();
        }
    }

    //
    // When the projectile appears, set its shouldExplode variable
    // to match the setting in the Inspector.
    //
    private void OnBecameVisible()
    {
        shouldExplode = explodeOnHit;
    }

    //
    // If the projectile has gone off screen, it should not explode.
    //
    private void OnBecameInvisible()
    {
        shouldExplode = false;
    }

    //
    // If the projectile collides with anything,
    // check if it should explode and deactivate it
    // so that it can be reused by ObjectPool.
    //
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy();
    }

    //
    // Instantiate an explosion if necessary. Then deactivate the object
    // so that it can be reused by ObjectPool.
    //
    private void Destroy()
    {
        if (shouldExplode)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
}