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
    
    public ShieldController shield;

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

    //
    // Initialisation
    //
    public override void Awake()
    {
        base.Awake();
        gameObject.name = "Player";

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

    public void ResetPosition(Vector2 position)
    {
        transform.position = position;
		transform.rotation = Quaternion.identity;
		rb.velocity = Vector2.zero;
    }

    public void Kill()
    {
        if(!shield.gameObject.activeSelf)
            DoKillSequence();
    }

    //
    // (Required by IKillable)
    // Player kill sequence.
    //
    public void DoKillSequence()
    {
        // Instantiate the explosion prefab
        Instantiate(explosion, transform.position, Quaternion.identity);

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
    }
}