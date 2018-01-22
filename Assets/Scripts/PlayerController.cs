using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int rotScaler;
    public float thrustScaler;
    private bool isAccelerating = false;

    public int lives = 3;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteSwitcher spriteSwitcher;
    private SpriteRenderer rend;
    private Vector2 velocity;

    public GameObject prefabWeaponBullet;
    private Transform anchorMainGun;

    public Sprite[] fragmentSprites;
    public GameObject fragmentPrefab;

    private GameManager gameManager;

    private AudioSource audioSource;
    public AudioClip laser;
    public AudioClip destroyed;
    public AudioClip engine;

    public bool isAlive;



    void Awake()
    {
        // Get references to components
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        spriteSwitcher = GetComponentInChildren<SpriteSwitcher>();
        anchorMainGun = transform.Find("AnchorMainGun");
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = engine;

        // We only read the player input when it's alive,
        // i.e. not going through the death animation
        isAlive = true;
    }



    void Update()
    {
        if (isAlive)
        {
            GetUserInput();   
            if (isAccelerating)
            {
                audioSource.PlayOneShot(engine);
            }
        }
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
        // Collision with asteroid
        if (collision.gameObject.tag == "Asteroid")
        {
            // Play explosion sound
            audioSource.Stop();
            audioSource.PlayOneShot(destroyed);

            // Hide the player, disable its collider & keyboard input
            rend.enabled = false;
            col.enabled = false;
            isAccelerating = false;

            // Reduce 1 life
            isAlive = false;
            lives -= 1;

            // Get the relative velocity of the collision
            float vel = collision.relativeVelocity.sqrMagnitude * 15;

            // Instantiate the fragments, pull them apart randomly
            foreach (Sprite sprite in fragmentSprites)
            {
                GameObject _go = Instantiate(fragmentPrefab, transform.position, transform.rotation);
                _go.GetComponent<SpriteRenderer>().sprite = sprite;
                Rigidbody2D _rb = _go.GetComponent<Rigidbody2D>();
                Vector2 randomVector = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

                // We add the velocity at impact to the random vector
                // This looks more natural
                _rb.AddForce(randomVector + velocity, ForceMode2D.Impulse);
                _rb.AddTorque(vel);  
            }

            gameManager.PlayerDied();

            if (lives == 0)
            {
                gameManager.GameOver();
                Destroy(gameObject, 3.0f);
            }
            else
            {
                Invoke("Respawn", 3.0f);    
            }
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
        while (gameManager.centerIsFree == false)
        {
            yield return null;
        }
        // Reenable the player when it's clear
        rend.enabled = true;
        col.enabled = true;
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        isAlive = true;
    }
     


    private void GetUserInput()
    {
        // Acceleration
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            spriteSwitcher.SpriteThrust();
            isAccelerating = true;
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
            Instantiate(prefabWeaponBullet, anchorMainGun.position, transform.rotation);
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