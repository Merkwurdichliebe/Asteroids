using UnityEngine;
using System;
using Random = UnityEngine.Random;


public class AsteroidController : Entity, IKillable
{
    // Reference for asteroid sprites variations
    public Sprite[] sprite;

    // Reference caching
    // private CircleCollider2D col;
    private AudioSource audioSource;

    private ParticleSystem ps;

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
            pointValue = (phase + 1) * 2;
            gameObject.name = "Asteroid (Phase " + phase + ")";
        }
    }

    // Static variable for counting how many
    // asteroids we've instantiated
    public static int countAsteroids = 0;


    public static event Action<Entity, Transform, int> OnDestroyed;

    public delegate void MessageEvent();
    public static event MessageEvent OnLastAsteroidDestroyed;

    public static event Action<Entity> OnScorePoints;



    public override void Awake()
    {
        base.Awake();
        // Cache needed Components
        // col = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();

        // ps = GetComponent<ParticleSystem>();

        // Set a random sprite variation
        rend.sprite = sprite[Random.Range(0, 3)];

        // Increase static asteroid count with each instantiation
        countAsteroids += 1;

        Phase = 0;
    }



    void Start()
    {
        // Spawn new asteroids in a random range above the player
        // Smaller asteroids ignore this and spawn where the bigger ones break
        if (phase == 0) 
        {
            transform.position = new Vector2(Random.Range(-15, 15), Random.Range(3, 6)); 
        }

        // Set the mass to be proportionate to the asteroid size
        // This makes inter-asteroid collisions more realistic
        rb.mass = 1 / (phase + 1);

        // SetActive(true);

        // Give the asteroid a random force and torque
        float dirX = Random.Range(-1f, 1f);
        float dirY = Random.Range(-1f, 1f);
        rb.AddRelativeForce(new Vector2(dirX, dirY) * (2 + GameManager.level * 0.5f) * rb.mass, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-1 * rb.mass, 1 * rb.mass), ForceMode2D.Impulse);
    }



    public void Kill()
    {
        // Spawn new asteroids, unless they are too small
        if (phase < 2)
        {
            SpawnAsteroid(phase + 1);
            SpawnAsteroid(phase + 1);
        }


        countAsteroids -= 1;

        // Play explosion sound
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();

        SetActive(false);

        if (OnDestroyed != null) OnDestroyed(this, gameObject.transform, pointValue);
        if (countAsteroids == 0 && OnLastAsteroidDestroyed != null) OnLastAsteroidDestroyed();
        Destroy(gameObject, 3.0f);
    }



    public void SpawnAsteroid(int phase)
    {
        // Instantiate the asteroid and set its phase from the method argument
        GameObject ast = Instantiate(gameObject, Vector2.zero, Quaternion.identity);
        AsteroidController astController = ast.GetComponent(typeof(AsteroidController)) as AsteroidController;
        astController.Phase = phase;

        // Position and rotation variations
        float x = Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f);
        float y = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
        float rot = Random.Range(0f, 1f);

        ast.transform.position = new Vector2(x, y);
        ast.transform.Rotate(new Vector3(0, 0, rot)); 
    }



    private void ScorePoints()
    {
        OnScorePoints(this);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objTag = collision.gameObject.tag;

        if (objTag == "PlayerProjectile")
        {
            collision.gameObject.SetActive(false);
            ScorePoints();
            Kill();
        }
    }
}
