using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    private float rotScaler = 5.0f;
    private float thrustScaler = 0.5f;
    private bool isAccelerating = false;

    public int lives = 3;

    private SpriteSwitcher spriteSwitcher;
    private Vector2 velocity;

    public GameObject pfBullet;
    private Transform anchorMainGun;

    public Sprite[] fragmentSprites;
    public GameObject fragmentPrefab;

    private AudioSource audioSource;
    public AudioClip laser;
    public AudioClip destroyed;
    public AudioClip engine;

    public static event DelegateEvent OnPlayerDied;
    public static event DelegateEvent OnPlayerLivesZero;



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
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collision with asteroid or UFO
        if (collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "Enemy")
        {
            Die(collision.relativeVelocity.sqrMagnitude * 15);
        }
    }



    public void Die(float impactMagnitude)
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
        lives -= 1;

        // Get the relative velocity of the collision
        float vel = impactMagnitude;

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

        OnPlayerDied();

        if (lives == 0)
        {
            OnPlayerLivesZero();
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
        // Don't do anything while the center is not clear of asteroids
        while (GameManager.CenterIsFree == false)
        {
            yield return null;
        }
        // Reenable the player when it's clear
        rend.enabled = true;
        col.enabled = true;
        rb.isKinematic = false;
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        isAlive = true;
    }


    public override void HitByEnemy()
    {
        base.HitByEnemy();
        Die(10);
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
            Instantiate(pfBullet, anchorMainGun.position, transform.rotation);
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