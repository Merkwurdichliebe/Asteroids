using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class PlayerController : Entity, IKillable
{
    private float rotScaler = 5.0f;
    private float thrustScaler = 0.5f;
    private bool isAccelerating = false;

    public int livesLeft = 3;

    private SpriteSwitcher spriteSwitcher;
    private Vector2 velocity;

    public Projectile PrefabProjectile;
    private Transform anchorMainGun;

    public Sprite[] fragmentSprites;
    public GameObject fragmentPrefab;

    private AudioSource audioSource;
    public AudioClip laser;
    public AudioClip destroyed;
    public AudioClip engine;

    private bool centerIsOccupied;

    public static event Action<int> OnPlayerDestroyed;
    public static event Action<float> OnPlayerSpeedChanged;



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
    }

    private void OnEnable()
    {
        GameManager.OnCenterClear += CenterIsClear;
        GameManager.OnCenterOccupied += CenterIsOccupied;
    }

    private void CenterIsClear() { centerIsOccupied = false; }
    private void CenterIsOccupied() { centerIsOccupied = true; }

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

        // Hide the player, disable its collider & keyboard input
        rend.enabled = false;
        col.enabled = false;
        rb.isKinematic = true;
        isAccelerating = false;

        // Reduce 1 life
        isAlive = false;
        livesLeft -= 1;

        // Get the relative velocity of the collision
        float vel = 20f;

        // Instantiate the fragments, pull them apart randomly
        foreach (Sprite sprite in fragmentSprites)
        {
            GameObject _go = Instantiate(fragmentPrefab, transform.position, transform.rotation);
            _go.GetComponent<SpriteRenderer>().sprite = sprite;
            Rigidbody2D _rb = _go.GetComponent<Rigidbody2D>();
            Vector2 randomVector = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

            // We add the velocity at impact to the random vector
            // This looks more natural
            _rb.AddForce((randomVector + velocity) * 1.5f, ForceMode2D.Impulse);
            _rb.AddTorque(vel);
        }

        OnPlayerDestroyed(livesLeft);

        if (livesLeft == 0)
        {
            Destroy(gameObject, 3.0f);
        }
        else
        {
            Invoke("Respawn", 3.0f);
        }
    }


    public void Respawn()
    {
        // We use this to avoid spawning the player
        // when asteroids are too close
        StartCoroutine("RespawnWhenCenterIsFree");
    }



    IEnumerator RespawnWhenCenterIsFree()
    {
        // Don't do anything while the center is not clear
        while (centerIsOccupied) { yield return null; }

        // Reenable the player when it's clear
        rend.enabled = true;
        col.enabled = true;
        rb.isKinematic = false;
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        isAlive = true;
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        string goTag = collision.gameObject.tag;

        if (goTag == "Asteroid" || goTag == "Enemy" || goTag == "EnemyProjectile")
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
            Instantiate(PrefabProjectile, anchorMainGun.position, transform.rotation);
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