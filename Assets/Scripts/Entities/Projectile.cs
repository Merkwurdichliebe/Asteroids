using UnityEngine;

public class Projectile : MonoBehaviour {
    
    public float speed;
    public float lifespan;

    private Rigidbody2D rb;

    void Awake()
    {
        // Get the Rigidbody2D when instantiated
        rb = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
    }



    void Start()
    {
        // Add forward impulse
        rb.AddRelativeForce(Vector2.up * speed / 10, ForceMode2D.Impulse);

        // Limit the bullet's range
        Destroy(gameObject, lifespan);
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
