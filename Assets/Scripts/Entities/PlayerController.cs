using System;
using System.Collections;
using UnityEngine;

public class PlayerController : Entity, IKillable
{
    //
    // Inspector fields 
    //
    [Header("When destroyed")]
    public GameObject explosion;

    [Header("Starting stats")]
    [Range(1, 10)]
    public int livesAtStart;

    //
    // Private fields
    //
    private int _livesLeft;
    private bool centerIsClear;
    private bool _activeInScene;
    private IMove moveComponent;
    private IFire fireComponent;

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
            _activeInScene = value;
            rend.enabled = value;
            col.enabled = value;
            rb.isKinematic = !value;
            // moveComponent.enabled = value;
            fireComponent.FiringEnabled = value;
            if (value)
            {
                if (OnPlayerSpawned != null) { OnPlayerSpawned(); }
            }
            else
            {
                if (OnPlayerDespawned != null) { OnPlayerDespawned(); }
            }
            Debug.Log("[PlayerController/PropertyActiveInScene] " + gameObject.name + " : " + value);
        }
    }

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
    // Events
    //
    public static Action OnPlayerSpawned;
    public static Action OnPlayerDespawned;
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
        Lives = livesAtStart;
        moveComponent = GetComponent<IMove>();
        fireComponent = GetComponent<IFire>();
        centerIsClear = true;
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
    //
    void HandleCenterIsClear(bool clear) { centerIsClear = clear; }


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
        ActiveInScene = false;

        // Reduce one life
        Lives -= 1;

        // Fire events
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
        ActiveInScene = true;
        transform.position = new Vector2(0, 0);
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;
    }
}