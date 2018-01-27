using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : Entity, IKillable
{
    public static event Action<int> OnPlayerDestroyed;
    public static event Action<float> OnPlayerSpeedChanged;

    public int livesLeft = 3;
    public Projectile prefabProjectile;
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
        Assert.IsNotNull(prefabProjectile);

        // We only read the player input when it's alive,
        // i.e. not going through the death animation
        isAlive = true;

        gameObject.name = "Player";
    }



    void OnEnable()
    {
        GameManager.OnCenterClear += CenterIsClear;
        GameManager.OnCenterOccupied += CenterIsOccupied;
    }

    void OnDisable()
    {
        GameManager.OnCenterClear -= CenterIsClear;
        GameManager.OnCenterOccupied -= CenterIsOccupied;
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
        SetActive(false);
        isAccelerating = false;

        // Reduce 1 life
        livesLeft -= 1;

        GetComponent<FragmentExploder>().Explode();
        OnPlayerDestroyed(livesLeft);

        if (livesLeft > 0) StartCoroutine(Respawn());
    }



    IEnumerator Respawn()
    {
        // Wait 3 seconds befor respawn
        yield return new WaitForSeconds(3.0f);

        // Don't do anything while the center is not clear
        while (centerIsOccupied) { yield return null; }

        // Reenable the player when it's clear
        SetActive(true);
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
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
            Instantiate(prefabProjectile, anchorMainGun.position, transform.rotation);
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