using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int RotationScaler = 5;
    public int ThrustScaler = 2;

    private Rigidbody2D rb;
    private SpriteSwitcher spriteSwitcher;
    private SpriteRenderer sr;

    public GameObject prefabWeaponBullet;
    private Transform anchorMainGun;

    private GameManager gameManager;

    private AudioSource audiosource;
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
        audiosource = GetComponent<AudioSource>();
        isAlive = true;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            spriteSwitcher.SpriteThrust();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            spriteSwitcher.SpriteIdle();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddRelativeForce(Vector2.up * ThrustScaler, ForceMode2D.Force);
            audiosource.PlayOneShot(engine);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.transform.Rotate(Vector3.forward * RotationScaler);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.transform.Rotate(Vector3.back * RotationScaler);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(prefabWeaponBullet, anchorMainGun.position, transform.rotation);
            audiosource.PlayOneShot(laser);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid" && isAlive)
        {
            audiosource.PlayOneShot(destroyed);
            sr.enabled = false;
            isAlive = false;
            gameManager.PlayerDied(this.gameObject);
        }
    }

    public void Respawn()
    {
        sr.enabled = true;
        isAlive = true;
        transform.position = new Vector2(0, 0);
        rb.velocity = new Vector2(0, 0);
    }
}
