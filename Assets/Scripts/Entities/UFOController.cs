using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
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
    private float firingInterval;


    public delegate void MessageEvent();
    public static event MessageEvent OnUFODestroyed;

    public static event Action<Entity> OnScorePoints;

    Coroutine fireCoroutine;

    private ParticleSystem ps;


    public override void Awake()
    {
        base.Awake();

        // Cache references
        player = GameObject.FindWithTag("Player");
        audiosource = GetComponent<AudioSource>();
        rend = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        Assert.IsNotNull(player);

        // Instantiate the projectile pool and start deactivated
        projectilePool = Instantiate(prefabProjectilePool);
        SetActive(false);
        ps = GetComponentInChildren<ParticleSystem>();
    }



    private void OnEnable()
    {
        GameManager.OnLevelStarted += StartUFOSpawner;
        GameManager.OnLevelStarted += UpdateStats;
        PlayerController.OnPlayerLivesZero += CleanUp;
    }



    private void OnDisable()
    {
        GameManager.OnLevelStarted -= StartUFOSpawner;
        GameManager.OnLevelStarted -= UpdateStats;
        PlayerController.OnPlayerLivesZero -= CleanUp;
    }



    void CleanUp()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }



    void Spawn()
    {
        SetActive(true);
        MoveToCenter();

        // Play sound
        audiosource.clip = soundUFOEngine;
        audiosource.loop = true;
        audiosource.volume = 1.0f;
        audiosource.pitch = 0.5f;
        audiosource.Play();

        pointValue = GameManager.level * 20;

        fireCoroutine = StartCoroutine(Fire());
    }



    void Despawn()
    {
        audiosource.Stop();
        // transform.position = new Vector2(1000, 1000);
        rb.velocity = Vector2.zero;
        if (fireCoroutine != null) StopCoroutine(fireCoroutine);
        StartUFOSpawner();
    }



    void StartUFOSpawner()
    {
        StopAllCoroutines();
        if (gameObject.activeInHierarchy) StartCoroutine(Spawner());
    }



    IEnumerator Spawner()
    {
        // Allow at least 2 seconds between death and respawn
        yield return new WaitForSeconds(3.0f);

        // If the random value is higher than the probability,
        // wait some more (spawnFrequency in seconds)
        while (Random.value > spawnProbability)
        {
            yield return new WaitForSeconds(spawnFrequency);    
        }

        // Only then, Spawn the UFO
        Spawn();
    }



    void UpdateStats()
    {
        // UFO gets more precise as level increases
        firingPrecision.x = Random.Range(0, Mathf.Clamp(1 - GameManager.level / 10, 0, 1.0f));
        firingPrecision.y = Random.Range(0, Mathf.Clamp(1 - GameManager.level / 10, 0, 1.0f));
        firingInterval = Mathf.Clamp(3 - GameManager.level / 5 * firingFrequency, 1.0f, 3.0f);
    }



    IEnumerator Fire()
    {
        while(true)
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
            // projectile.tag = "EnemyProjectile";
            projectile.SetActive(true);

            yield return new WaitForSeconds(firingInterval);
        }
    }



    private void ScorePoints()
    {
        OnScorePoints(this);
    }



    public void Kill()
    {
        ps.Play();
        SetActive(false);
        audiosource.clip = soundUFOExplosion;
        audiosource.loop = false;
        audiosource.volume = 0.2f;
        audiosource.pitch = 0.5f;
        audiosource.Play();
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



    // We need this in order to handle when the UFO leaves the screen
    private void OnBecameInvisible()
    {
        Despawn();
    }
}
