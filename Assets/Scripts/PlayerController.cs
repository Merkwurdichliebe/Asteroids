using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int rotScaler;
    public float thrustScaler;
    private bool isAccelerating = false;

    private Rigidbody2D rb;
    private SpriteSwitcher spriteSwitcher;
    private SpriteRenderer sr;

    public GameObject prefabWeaponBullet;
    private Transform anchorMainGun;

    private GameManager gameManager;

    private AudioSource audioEngine;
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
        audioEngine.clip = engine;
        isAlive = true;
    }



    void Update()
    {
        if (isAlive)
        {
            GetUserInput();   
        }

        if (isAccelerating)
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
        if (isAccelerating)
        {
            rb.AddRelativeForce(Vector2.up * thrustScaler, ForceMode2D.Force);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid" && isAlive)
        {
            sr.enabled = false;
            isAlive = false;
            isAccelerating = false;
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
            isAccelerating = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            spriteSwitcher.SpriteIdle();
            isAccelerating = false;
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
