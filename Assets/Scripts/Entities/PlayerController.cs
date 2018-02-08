using System;
using System.Collections;
using UnityEngine;

public class PlayerController : Entity, IKillable
{
    // -----------------------------------------------------------------------------
    // Inspector fields
    // -----------------------------------------------------------------------------

    [Header("When destroyed")]
    public GameObject explosion;

    [Header("Starting stats")]
    [Range(1, 10)]
    public int livesAtStart;

    private int _livesLeft;
    private bool centerIsClear;

    private IMove moveComponent;
    private IFire fireComponent;

    private bool _activeInScene;

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
            Debug.Log("[SetActiveInScene] " + gameObject.name + " : " + value);
        }
    }

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



    public override void Awake()
    {
        base.Awake();
        gameObject.name = "Player";
        Lives = livesAtStart;
        moveComponent = GetComponent<IMove>();
        fireComponent = GetComponent<IFire>();
    }



    void OnEnable()
    {
        SpawnSafeZoneManager.OnSpawnSafeZoneCleared += HandleCenterIsClear;
    }



    void OnDisable()
    {
        SpawnSafeZoneManager.OnSpawnSafeZoneCleared -= HandleCenterIsClear;
    }



    // Event handler for when the center spawn safe zone is clear.
    void HandleCenterIsClear(bool b) { centerIsClear = b; }



    // (Required by IKillable)
    // Player kill sequence.
    public void Kill()
    {
        // Instantiate the explosion prefab
        Instantiate(explosion, transform.position, Quaternion.identity);

        // Hide the player, disable its collider & keyboard input
        ActiveInScene = false;

        // Reduce one life
        Lives -= 1;

        // Fire events
        if (OnPlayerDestroyed != null) { OnPlayerDestroyed(); }

        // Check if we should respawn.
        // Otherwise the game is over and we can destroy the player object.
        if (_livesLeft > 0) {
            StartCoroutine(RespawnInSeconds(3));
        }
        else
        {
            if (OnPlayerLivesZero != null) { OnPlayerLivesZero(); }
            Destroy(gameObject, 3);
        }
    }



    // Wait a while before respawning, to allow for the explosion effect
    // to finish.
    IEnumerator RespawnInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(WaitForCenterClear());
    }



    // Wait until the center spawn safe zone is clear,
    // so as not to spawn right next to an asteroid or enemy.
    IEnumerator WaitForCenterClear()
    {
        while (!centerIsClear) { yield return null; }
        Spawn();
    }



    // Reactivate the player and reset its transform and velocity to zero.
    public void Spawn()
    {
        ActiveInScene = true;
        transform.position = new Vector2(1, 1);
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;
    }
}