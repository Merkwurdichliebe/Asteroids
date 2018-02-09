using UnityEngine;
using System.Collections;

// The Projectile class is designed to be instantiated as part of
// an object pool. It should be disabled rather than destroyed.

public class Projectile : MonoBehaviour {

    //
    // Inspector fields 
    //
    public float speed;
    public float lifespan;
    public bool explodeOnHit;
    public GameObject explosion;

    //
    // Private fields 
    //
    private Rigidbody2D rb;
    private bool shouldExplode;

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
    // Launch the projectile immediately after it's instantiated and enabled. 
    //
    void OnEnable()
    {
        StartCoroutine(Launch());
    }

    //
    // Deactivate the projectile when its lifespan has been reached.
    // Instantiate an explosion if necessary. Then deactivate the object
    // so that it can be reused by ObjectPool.
    //
    IEnumerator Launch()
    {
        rb.AddRelativeForce(Vector2.up * speed / 10, ForceMode2D.Impulse);
        yield return new WaitForSeconds(lifespan);
        if (shouldExplode)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

    //
    // When the projectile appears, set its shouldExplode variable
    // to match the setting in the Inspector.
    //
    private void OnBecameVisible()
    {
        Debug.Log("[Projectile/OnBecameVisible]");
        shouldExplode = explodeOnHit;
    }

    //
    // If the projectile has gone off screen, it should not explode.
    //
    private void OnBecameInvisible()
    {
        Debug.Log("[Projectile/OnBecameInvisible]");
        shouldExplode = false;
    }

    //
    // If the projectile collides with anything,
    // check if it should explode and deactivate it
    // so that it can be reused by ObjectPool.
    //
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (shouldExplode)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
}