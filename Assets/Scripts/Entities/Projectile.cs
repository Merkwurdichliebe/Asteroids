using UnityEngine;
using System.Collections;

// The Projectile class is designed to be instantiated as part of
// an object pool. It should be disabled rather than destroyed.

public class Projectile : MonoBehaviour {
    
    public float speed;
    public float lifespan;
    public bool explodeOnHit;
    public GameObject explosion;

    private Rigidbody2D rb;
    private bool shouldExplode;



    void Awake()
    {
        rb = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
        if (explodeOnHit && explosion == null)
        {
            Debug.LogError("[Projectile/Awake] Explode selected but none is assigned.");
        }
    }



    void OnEnable()
    {
        StartCoroutine(Launch());
    }



    // Deactivate the projectile when it lifespan is reached.
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

    private void OnBecameVisible()
    {
        Debug.Log("[Projectile/OnBecameVisible]");
        shouldExplode = explodeOnHit;
    }

    private void OnBecameInvisible()
    {
        Debug.Log("[Projectile/OnBecameInvisible]");
        shouldExplode = false;
    }

    // Deactivate the projectile when it collides with anything.
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (shouldExplode)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
}
