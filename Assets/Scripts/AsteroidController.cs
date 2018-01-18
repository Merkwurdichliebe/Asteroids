using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{

    public Sprite[] sprite;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private int phase;

    public int Phase
    {
        get
        {
            return phase;
        }

        set
        {
            phase = value;
            float newScale = 1.0f / phase;
            transform.localScale = new Vector2(newScale, newScale);
            // rb.mass = 1 / phase;
        }
    }

    void Awake()
    {
        sr = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
        sr.sprite = sprite[Random.Range(0, 3)];
        rb = GetComponent<Rigidbody2D>();

        // Position and rotation variations
        float x = Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f);
        float y = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
        float rot = Random.Range(0f, 1f);

        transform.position = new Vector2(x, y);
        transform.Rotate(new Vector3(0, 0, rot));
    }

    void Start()
    {
        // Gets its Rigidbody2D and give a random force and torque
        float dirX = Random.Range(-1f, 1f);
        float dirY = Random.Range(-1f, 1f);
        rb.AddRelativeForce(new Vector2(dirX, dirY) * 80 * phase, ForceMode2D.Force);
        rb.AddTorque(Random.Range(-10, 10), ForceMode2D.Force);
    }

}
