using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : Entity
{
    // Reference for asteroid sprites variations
    public Sprite[] sprite;

    // Reference caching
    private CircleCollider2D col;
    private AudioSource audioSource;

    // Phase Property
    // Asteroids start at Phase 0
    // and go through 1 & 2 until completely destroyed
    // When set, adjust the scale of the asteroid
    private int phase = 0;
    public int Phase
    {
        get { return phase; }
        set
        {
            phase = value;
            float newScale = 1.0f / Mathf.Pow(2, phase);
            transform.localScale = new Vector2(newScale, newScale);
        }
    }

    // Static variable for counting how many
    // asteroids we've instantiated
    public static int countAsteroids = 0;



    public override void Awake()
    {
        base.Awake();
        // Cache needed Components
        col = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();

        // Set a random sprite variation
        rend.sprite = sprite[Random.Range(0, 3)];

        // Increase static asteroid count with each instantiation
        countAsteroids += 1;
    }



    void Start()
    {
        if (phase == 0) 
        {
            transform.position = new Vector2(Random.Range(-15, 15), Random.Range(3, 6)); 
        }

        rb.mass = 1 / (phase + 1);

        // Gets its Rigidbody2D and give a random force and torque
        float dirX = Random.Range(-1f, 1f);
        float dirY = Random.Range(-1f, 1f);
        rb.AddRelativeForce(new Vector2(dirX, dirY) * (2 + GameManager.level * 0.5f) * rb.mass, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-1 * rb.mass, 1 * rb.mass), ForceMode2D.Impulse);
    }



    public void Break()
    {
        if (phase < 2)
        {
            SpawnAsteroid(phase + 1);
            SpawnAsteroid(phase + 1);
        }
        gameManager.ScorePoints((phase + 1) * 2);
        countAsteroids -= 1;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        rend.enabled = false;
        col.enabled = false;
        if (countAsteroids == 0)
        {
            gameManager.NextLevel();
        }
        Destroy(gameObject, 4.0f);
    }

    override public void HitByPlayer()
    {
        base.HitByPlayer();
        Break();
    }

    public void SpawnAsteroid(int phase)
    {
        // Instantiate the asteroid and set its phase
        GameObject ast = Instantiate(gameObject, Vector2.zero, Quaternion.identity);
        //Debug.Log("After Instantiate " + countAsteroids);
        AsteroidController astController = ast.GetComponent(typeof(AsteroidController)) as AsteroidController;
        astController.Phase = phase;

        // Position and rotation variations
        float x = Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f);
        float y = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
        float rot = Random.Range(0f, 1f);

        ast.transform.position = new Vector2(x, y);
        ast.transform.Rotate(new Vector3(0, 0, rot));
    }
}
