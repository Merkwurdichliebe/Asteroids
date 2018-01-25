using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : Entity {
    
    public GameObject pfBullet;
    public AudioClip soundUFOEngine;
    public AudioClip soundUFOExplosion;

    private GameObject player;
    private AudioSource audiosource;
    private Vector3 firingPrecision;



    public override void Awake()
    {
        base.Awake();

        // Get reference to the player
        player = GameObject.FindWithTag("Player");
        audiosource = GetComponent<AudioSource>();
        rend = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // Randomly choose left or right of screen
        float x = (Random.value < 0.5f) ? -10 : 10;

        // Randomly select a vertical position
        float y = Random.Range(-6, 6);

        // Set the transform
        transform.position = new Vector2(x, y);
        transform.rotation = Quaternion.identity;
    }



    void Start()
    {
        // Calculate vector to center to screen
        Vector2 vector = Vector3.zero - transform.position;

        // Move towards the player
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
        InvokeRepeating("Fire", 2.0f, Mathf.Clamp(3 - GameManager.level / 5, 1.0f, 3.0f));
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
            Instantiate(pfBullet, transform.position, rotation);
        }
    }



    override public void HitByPlayer()
    {
        base.HitByPlayer();
        Die();
        EventManager.MessageScorePoints(GameManager.level * 20);
    }



    private void Die()
    {
        audiosource.clip = soundUFOExplosion;
        audiosource.loop = false;
        audiosource.volume = 0.2f;
        audiosource.pitch = 0.5f;
        audiosource.Play();
        isAlive = false;
        col.enabled = false;
        rend.enabled = false;
        CancelInvoke("Fire");
    }
    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collision with asteroid
        if (collision.gameObject.tag == "Asteroid") Die();
    }



    private void OnBecameInvisible()
    {
        Destroy(gameObject, 2.0f);
    }



    private void OnDestroy()
    {
        EventManager.MessageUFODestroyed();
    }
}
