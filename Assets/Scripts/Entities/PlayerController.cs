using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : Entity, IKillable
{
    
    public static event Action<float> OnPlayerSpeedChanged;

    public static Action OnPlayerSpawned;
    public static Action OnPlayerDestroyed;
    public static Action<int> OnPlayerLivesChanged;
    public static Action OnPlayerLivesZero;

    private int livesLeft;

    public ObjectPool prefabProjectilePool;
    private ObjectPool projectilePool;
    public AudioClip laser;
    public AudioClip destroyed;
    public AudioClip engine;

    private float rotScaler = 5.0f;
    private float thrustScaler = 0.5f;
    private bool isAccelerating = false;
    private SpriteSwitcher spriteSwitcher;
    private Vector2 velocity;
    private Transform anchorMainGun;
    private AudioSource audioSource;
    private bool centerIsOccupied;

    private ParticleSystem ps;

    public int Lives
    {
        get
        {
            return livesLeft;
        }
        set
        {
            livesLeft = value;
            OnPlayerLivesChanged(livesLeft);
        }
    }

    public override void Awake()
    {
        base.Awake();
        // Get references to components
        // col = GetComponent<Collider2D>();
        spriteSwitcher = GetComponentInChildren<SpriteSwitcher>();
        anchorMainGun = transform.Find("AnchorMainGun");
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = engine;
        audioSource.volume = 1.0f;

        // We only read the player input when it's alive,
        // i.e. not going through the death animation
        isAlive = true;

        gameObject.name = "Player";

        projectilePool = Instantiate(prefabProjectilePool);
        ps = GetComponentInChildren<ParticleSystem>();
    }



    void OnEnable()
    {
        SpawnSafeZoneManager.OnSpawnSafeZoneClear += CenterIsClear;
        SpawnSafeZoneManager.OnSpawnSafeZoneOccupied += CenterIsOccupied;
    }

    void OnDisable()
    {
        SpawnSafeZoneManager.OnSpawnSafeZoneClear -= CenterIsClear;
        SpawnSafeZoneManager.OnSpawnSafeZoneOccupied -= CenterIsOccupied;
    }

    void CenterIsClear() { centerIsOccupied = false; }
    void CenterIsOccupied() { centerIsOccupied = true; }



    void Update()
    {
        if (isAlive) GetUserInput();
    }



    void FixedUpdate()
    {
        if (isAccelerating)
        {
            rb.AddRelativeForce(Vector2.up * thrustScaler, ForceMode2D.Force);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 9.9f);
            velocity = rb.velocity; // Cached for velocity at impact
            OnPlayerSpeedChanged(velocity.magnitude);
        }
    }



    public void Kill()
    {
        // Play explosion sound
        audioSource.Stop();
        audioSource.volume = 0.7f;
        audioSource.loop = false;
        audioSource.PlayOneShot(destroyed);

        ps.Play();

        // Hide the player, disable its collider & keyboard input
        SetActive(false);
        isAccelerating = false;

        // Reduce 1 life
        livesLeft -= 1;

        // Explode using velocity just before impact
        GetComponent<FragmentExploder>().Explode(velocity);
        OnPlayerDestroyed();
        OnPlayerLivesChanged(livesLeft);

        if (livesLeft > 0)
        {
            StartCoroutine(Respawn());   
        }
        else
        {
            OnPlayerLivesZero();
            Destroy(gameObject, 3);
        }
    }



    IEnumerator Respawn()
    {
        // Wait 3 seconds before respawn
        yield return new WaitForSeconds(3.0f);

        // Don't do anything while the center is not clear
        while (centerIsOccupied) { yield return null; }

        // Reenable the player when it's clear
        SetActive(true);
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        OnPlayerSpawned();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        string objTag = collision.gameObject.tag;

        if (objTag == "EnemyProjectile")
        {
            collision.gameObject.SetActive(false);
            Kill();
        }

        if (objTag == "PowerUp")
        {
            CollectPowerUp(collision.gameObject);
        }
    }



    private void CollectPowerUp(GameObject obj)
    {
        PowerUpController pu = obj.GetComponent<PowerUpController>();
        if (pu.powerUpType == PowerUpType.FrontAndBack)
        {
            Debug.Log("Yes !");
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        string goTag = collision.gameObject.tag;

        if (goTag == "Asteroid" || goTag == "Enemy")
        {
            Kill();
        }
    }

    private void GetUserInput()
    {
        // Acceleration
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            spriteSwitcher.SpriteThrust();
            isAccelerating = true;
            audioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            spriteSwitcher.SpriteIdle();
            isAccelerating = false;
            audioSource.Stop();
        }

        // Fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectile = projectilePool.GetObject();
            if (projectile != null)
            {
                projectile.transform.position = anchorMainGun.position;
                projectile.transform.rotation = transform.rotation;
                // projectile.tag = "PlayerProjectile";
                projectile.SetActive(true);
            }
        }

        // Rotation
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.transform.Rotate(Vector3.forward * rotScaler);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.transform.Rotate(Vector3.back * rotScaler);
        }
    }
}