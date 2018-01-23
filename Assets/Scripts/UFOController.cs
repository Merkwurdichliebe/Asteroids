using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : Entity {

    public GameObject pfBullet;

    private GameObject player;
    private Rigidbody2D rb;

    void Awake()
    {
        // Get reference to the player and the RigidBody2D
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

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

        // Fire at the player
        InvokeRepeating("Fire", 2.0f, 1.0f);
    }

    private void Fire()
    {
        // Calculate vector to player
        Vector2 direction = player.transform.position - transform.position;

        // Calculate angle to player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Fire missile
        GameObject missile = Instantiate(pfBullet, transform.position, rotation);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    override public void HitByPlayer()
    {
        base.HitByPlayer();
        Debug.Log("UFO was hit by player");
    }
}
