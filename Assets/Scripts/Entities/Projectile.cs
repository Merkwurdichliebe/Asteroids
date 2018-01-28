using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    
    public float speed;
    public float lifespan;

    private Rigidbody2D rb;

    void Awake()
    {
        // Get the Rigidbody2D when instantiated
        rb = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
    }

    void OnEnable()
    {
        StartCoroutine(Launch());
    }

    IEnumerator Launch()
    {
        rb.AddRelativeForce(Vector2.up * speed / 10, ForceMode2D.Impulse);
        yield return new WaitForSeconds(lifespan);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }
}
