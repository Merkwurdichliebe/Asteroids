using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBullet : MonoBehaviour {

    public float speed = 1.0f;
    private Rigidbody2D rb;

    public float lifespan = 0.3f;
    private float timeAtSpawn;

    void Awake()
    {
        // Get the Rigidbody2D when instantiated
        rb = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
    }

	void Start ()
    {
        // Add forward impulse
        rb.AddRelativeForce(Vector2.up * speed * 10, ForceMode2D.Impulse);
        timeAtSpawn = Time.time;
	}

    void FixedUpdate()
    {
        // Destroy if left the screen
        Vector3 positionVP = Camera.main.WorldToViewportPoint(transform.position);
        if (positionVP.x > 1.0 || positionVP.x < 0 || positionVP.y > 1.0 || positionVP.y < 0)
        {
            Destroy(gameObject);
        }
        if (Time.time - timeAtSpawn > lifespan)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AsteroidController ast = collision.gameObject.GetComponent<AsteroidController>();

        if (ast != null)
        {
            ast.Break();
        }

        Destroy(gameObject);
    }
}