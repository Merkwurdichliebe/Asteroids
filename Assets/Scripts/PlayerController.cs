using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int rotScaler = 5;
    public int ThrustScaler = 2;
    private bool accelerating = false;

    private Rigidbody2D rb;
    private SpriteSwitcher spriteSwitcher;
    private SpriteRenderer sr;

    public GameObject prefabWeaponBullet;
    private Transform anchorMainGun;

    private GameManager gameManager;

    private AudioSource audioEngine;
    private AudioSource audioFX;
    public AudioClip laser;
    public AudioClip destroyed;
    public AudioClip engine;

    public bool isAlive;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteSwitcher = GetComponentInChildren<SpriteSwitcher>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anchorMainGun = transform.Find("AnchorMainGun");
        gameManager = FindObjectOfType<GameManager>();
        audioEngine = GetComponent<AudioSource>();
        audioFX = GetComponent<AudioSource>();
        audioEngine.clip = engine;
        isAlive = true;
    }



    void Update()
    {
        if (isAlive)
        {
            GetUserInput();   
        }

        if (accelerating)
        {
            audioEngine.PlayOneShot(engine);
        }
        else
        {
            audioEngine.Stop();
        }
    }



    void FixedUpdate()
    {
        if (accelerating)
        {
            rb.AddRelativeForce(Vector2.up * ThrustScaler, ForceMode2D.Force);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid" && isAlive)
        {
            sr.enabled = false;
            isAlive = false;
            gameManager.PlayerDied(this.gameObject);
        }
    }



    public void Respawn()
    {
        sr.enabled = true;
        isAlive = true;
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
    }

     

    private void GetUserInput()
    {
        // Acceleration
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            spriteSwitcher.SpriteThrust();
            accelerating = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            spriteSwitcher.SpriteIdle();
            accelerating = false;
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
