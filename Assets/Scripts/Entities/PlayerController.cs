using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity, IKillable
{
    //
    // Inspector fields 
    //
    [Header("When destroyed")]
    public GameObject explosion;

    //
    // Private fields
    //
    private int _livesLeft;
    private bool centerIsClear;
    private Vector2 spawnPosition;
    private EntitySpawnController player;

    //
    // Property: Player lives count
    //
    public int Lives
    {
        get { return _livesLeft; }
        set
        {
            _livesLeft = value;
            if (OnPlayerLivesChanged != null) { OnPlayerLivesChanged(_livesLeft); }
        }
    }

    //
    // List which holds all the child objects of the player
    // which are tagged with "Weapon".
    //
    public List<GameObject> Weapons { get; private set; }

    // 
    // Events
    //
    public static Action OnPlayerDestroyed;
    public static Action<int> OnPlayerLivesChanged;
    public static Action OnPlayerLivesZero;

    //
    // Initialisation
    //
    public override void Awake()
    {
        base.Awake();
        gameObject.name = "Player";
        centerIsClear = true;
        spawnPosition = Vector2.zero;
        player = GetComponent<EntitySpawnController>();

        // Build a list of all child gameobjects tagged with "Weapon"
        Weapons = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == "Weapon")
            {
                Weapons.Add(child);
            }
        }
    }

    //
    // Event subscriptions
    //
    void OnEnable()
    {
        SafeZone.OnSafeZoneClear += HandleCenterIsClear;
    }

    void OnDisable()
    {
        SafeZone.OnSafeZoneClear -= HandleCenterIsClear;
    }

    //
    // Event handler for when the center spawn safe zone is clear.
    // It repositions the player at the position where the safe zone
    // has repositioned itself and checked it is clear of hostiles.
    //
    void HandleCenterIsClear(bool clear, Vector2 zonePosition)
    {
        centerIsClear = clear;
        spawnPosition = zonePosition;
    }


    //
    // (Required by IKillable)
    // Player kill sequence.
    //
    public void Kill()
    {
        // Instantiate the explosion prefab
        Instantiate(explosion, transform.position, Quaternion.identity);

        // Stop and hide the player, disable its collider & keyboard input
        rb.velocity = Vector2.zero;
        player.ActiveInScene = false;

        // Reduce one life
        Lives -= 1;

        // Reset all weapons except for the Main Gun
        foreach (GameObject weapon in Weapons)
        {
            if (weapon.name != "Main Gun")
            {
                weapon.GetComponent<IFire>().IsEnabled = false;
            }
        }

        // Fire event
        if (OnPlayerDestroyed != null) { OnPlayerDestroyed(); }

        // Check if we should respawn.
        // Otherwise the game is over and we can destroy the player object.
        if (_livesLeft > 0) {
            SpawnInSeconds(3);
        }
        else
        {
            if (OnPlayerLivesZero != null) { OnPlayerLivesZero(); }
            Destroy(gameObject, 3);
        }
    }

    // Wait a while before respawning, to allow for the explosion effect
    // to finish.
    public void SpawnInSeconds(int seconds)
    {
        StartCoroutine(WaitForSeconds(seconds));
    }

    // Wait for a number of seconds,
    // then start the safe zone clear check.
    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(WaitForCenterClearCoroutine());
    }

    // Wait until the center spawn safe zone is clear,
    // so as not to spawn right next to an asteroid or enemy.
    IEnumerator WaitForCenterClearCoroutine()
    {
        while (!centerIsClear) { yield return null; }
        Spawn();
    }

    // Reactivate the player and reset its transform and velocity to zero.
    public void Spawn()
    {
        player.ActiveInScene = true;
        transform.position = spawnPosition;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;
    }
}