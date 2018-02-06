﻿using UnityEngine;

public class UFOController : Entity, IKillable, ICanScorePoints, ISpawnable
{

    public Spawner Spawner { get; set; }

    // -----------------------------------------------------------------------------
    // Inspector fields
    // -----------------------------------------------------------------------------

    // Explosion prefab to be instantiated when destroyed
    public GameObject explosion;

    // Base scoring values from which to set the PointValue property
    public int basePointValue;
    public bool displayPointsWhenKilled;

    // Required by ICanScorePoints
    public int PointValue
    {
        get { return _pointValue; }
        set { _pointValue = value; }
    }
    public bool DisplayPointsWhenKilled { get { return displayPointsWhenKilled; } }

    private int _pointValue;

    // Static property counting how many are in the scene
    public static int Count { get; private set; }



    public override void Awake()
    {
        base.Awake();
        gameObject.name = "UFO";
        Count += 1;
    }



    private void Start()
    {
        PointValue = GameManager.level * basePointValue;
    }



    private void OnEnable()
    {
        EventManager.Instance.OnPlayerLivesZero += CleanUp;
    }



    private void OnDisable()
    {
        EventManager.Instance.OnPlayerLivesZero -= CleanUp;
    }



    // (Required by IKillable)
    // UFO kill sequence.
    public void Kill()
    {
        Debug.Log("[UFOController/Kill]");
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }



    void CleanUp()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }



    // We need this in order to handle when the UFO leaves the screen.
    // The final steps are taken in OnDestroy().
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }



    // We decrease the count and fire the event only when destroyed.
    private void OnDestroy()
    {
        Count -= 1;
        EventManager.Instance.UFODestroyed();
        Spawner.NotifyDestroyed(this.gameObject);
    }



    // Destroy the UFO if it collides with asteroid or the player.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // string objTag = collision.gameObject.tag;
        if (collision.gameObject.CompareTag("Asteroid") || 
            collision.gameObject.CompareTag("Player"))
        {
            Kill();   
        }
    }



    // Projectiles are Triggers, not Colliders.
    // Notify the EventManager, then destroy the UFO.
    // The EventManager will get this instance of the script
    // as a ICanScorePoints interface.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            EventManager.Instance.EntityKilledByPlayer(this);
            Kill();
        }
    }
}
