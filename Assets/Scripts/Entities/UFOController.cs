using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class UFOController : Entity, IKillable
{
    public AudioClip soundUFOEngine;
    public AudioClip soundUFOExplosion;
    public float firingFrequency;
    public ObjectPool prefabProjectilePool;

    // For tuning
    public float spawnFrequency;
    public float spawnProbability;

    private ObjectPool projectilePool;

    private GameObject player;
    private AudioSource audiosource;
    private Vector3 firingPrecision;


    public delegate void MessageEvent();
    public static event MessageEvent OnUFODestroyed;

    public static event Action<Entity> OnScorePoints;


    public override void Awake()
    {
        base.Awake();

        // Get reference to the player
        player = GameObject.FindWithTag("Player");
        audiosource = GetComponent<AudioSource>();
        rend = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        pointValue = 20;
        projectilePool = Instantiate(prefabProjectilePool);
        SetActive(false);
    }



    private void OnEnable()
    {
        GameManager.OnLevelStarted += StartUFOSpawner;
    }



    public override void SetActive(bool active)
    {
        base.SetActive(active);
        if (active)
        {
            // Randomly choose left or right of screen
            float x = (Random.value < 0.5f) ? -10 : 10;

            // Randomly select a vertical position
            float y = Random.Range(-6, 6);

            // Set the transform
            transform.position = new Vector2(x, y);
            transform.rotation = Quaternion.identity;

            pointValue = GameManager.level * 20;

            // Calculate vector to center to screen
            Vector2 vector = Vector3.zero - transform.position;

            // Move towards center of screen
            rb.AddForce(vector * 10);

            audiosource.clip = soundUFOEngine;
            audiosource.loop = true;
            audiosource.volume = 1.0f;
            audiosource.pitch = 0.5f;
            audiosource.Play();

            // UFO gets more precise as level increases

            firingPrecision.x = Random.Range(0, Mathf.Clamp(1 - GameManager.level / 10, 0, 1.0f));
            firingPrecision.y = Random.Range(0, Mathf.Clamp(1 - GameManager.level / 10, 0, 1.0f));

            // Fire at the player, more frequently with level
            InvokeRepeating("Fire", 2.0f, Mathf.Clamp(3 - GameManager.level / 5 * firingFrequency, 1.0f, 3.0f));
        }
        else
        {
            audiosource.Stop();
            transform.position = new Vector2(1000, 1000);
            rb.velocity = Vector2.zero;
            CancelInvoke("Fire");
        }
    }


    void StartUFOSpawner()
    {
        InvokeRepeating("SpawnUFO", 3.0f, spawnFrequency);
    }



    void SpawnUFO()
    {
        if (Random.value < spawnProbability)
        {
            SetActive(true);
            CancelInvoke("SpawnUFO");
        }
    }



    private void Fire()
    {
        if (player != null)
        {
            // Calculate vector to player
            Vector2 direction = (player.transform.position - transform.position) + firingPrecision;

            // Calculate angle to player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Fire missile
            GameObject projectile = projectilePool.GetObject();
            projectile.transform.position = transform.position;
            projectile.transform.rotation = rotation;
            projectile.tag = "EnemyProjectile";
            projectile.SetActive(true);
        }
    }



    private void ScorePoints()
    {
        OnScorePoints(this);
    }



    public void Kill()
    {
        SetActive(false);
        audiosource.clip = soundUFOExplosion;
        audiosource.loop = false;
        audiosource.volume = 0.2f;
        audiosource.pitch = 0.5f;
        audiosource.Play();
        StartUFOSpawner();
        if (OnUFODestroyed != null) OnUFODestroyed();
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


    void OnCollisionEnter2D(Collision2D collision)
    {
        string objTag = collision.gameObject.tag;

        if (objTag == "Asteroid" || objTag == "Player")
        {
            Kill();   
        }
    }


    private void OnBecameInvisible()
    {
        SetActive(false);
    }
}
