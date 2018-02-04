using UnityEngine;
using System.Collections;

// The Projectile class is designed to be instantiated as part of
// an object pool. It should be disabled rather than destroyed.

public class Projectile : MonoBehaviour {
    
    public float speed;
    public float lifespan;

    private Rigidbody2D rb;



    void Awake()
    {
        rb = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
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
        gameObject.SetActive(false);
    }



    // Deactivate the projectile when it collides with anything.
    // FIXME should not deactivate when colliding with other projectiles
    void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }
}
