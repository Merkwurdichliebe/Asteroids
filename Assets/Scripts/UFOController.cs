using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : Entity {
    
    public GameObject pfBullet;

    private GameObject player;
    private AudioSource audiosource;
    private Vector3 firingPrecision;



    public override void Awake()
    {
        base.Awake();

        // Get reference to the player
        player = GameObject.FindWithTag("Player");
        audiosource = GetComponent<AudioSource>();

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
        // Calculate vector to player
        Vector2 vector = player.transform.position - transform.position;

        // Move towards the player
        rb.AddForce(vector * 10);


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



    private void OnBecameInvisible()
    {
        // TODO: Don't do both Destroys here and in HitByPlayer
        Destroy(gameObject, 3.0f);
    }



    override public void HitByPlayer()
    {
        base.HitByPlayer();
        audiosource.Play();
        isAlive = false;
        CancelInvoke("Fire");
        rb.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        rb.gameObject.GetComponent<Collider2D>().enabled = false;
        EventManager.MessageScorePoints(GameManager.level * 20);
        Destroy(gameObject, 3.0f);
    }

    private void OnDestroy()
    {
        EventManager.MessageUFODestroyed();
    }
}
