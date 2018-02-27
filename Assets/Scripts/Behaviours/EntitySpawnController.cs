using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawnController : MonoBehaviour {

    //
    // Inspector fields
    //
    [Header("Child objects to include")]
    public GameObject engine;
    public GameObject shield;

	//
	// Private fields
	//

	private bool _activeInScene;
    private IMove moveComponent;
    private IFire[] fireComponents;
    private SpriteRenderer rend;
    private Collider2D col;
    private Rigidbody2D rb;

	//
    // Property: Player active in scene.
    // The player is the only entity which doesn't get OnDestroy
    // when it is killed or hit. Instead, we hide it and disable its
    // rigidbody & collider here. This is because the Player handles its own
    // respawning checks and lives count, so we need to keep it alive.
    //
    public bool ActiveInScene
    {
        get
        {
            return _activeInScene;
        }
        set
        {
            // Enable/disable physics components
            _activeInScene = value;
            rend.enabled = value;
            col.enabled = value;
            rb.isKinematic = !value;
            
            // Enable/disable all child objects which have
            // IFire components attached
            foreach (IFire i in fireComponents)
            {
                ((Component)i).gameObject.SetActive(value);
            }

            // Enable/disable attached the IMove component
            // (this needs to be done through MonoBehaviour)
            ((MonoBehaviour)moveComponent).enabled = value;

            // Enable/disable the engine child object
            // (hide smoke when unspawned)
            engine.SetActive(value);
            // shield.SetActive(value);

            // TODO: the above is really ugly and should be streamlined

            // Fire event
            if (value)
            {
                if (OnPlayerSpawned != null) { OnPlayerSpawned(); }
            }
            else
            {
                if (OnPlayerDespawned != null) { OnPlayerDespawned(); }
            }
            // Debug.Log("[PlayerController/PropertyActiveInScene] " + gameObject.name + " : " + value);
        }
    }

	// 
    // Events
    //
    public static Action OnPlayerSpawned;
    public static Action OnPlayerDespawned;

    //
    // Initialization
    //
    private void Awake()
	{
		moveComponent = GetComponentInChildren<IMove>();
        fireComponents = GetComponentsInChildren<IFire>();
        rend = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
	}
}
