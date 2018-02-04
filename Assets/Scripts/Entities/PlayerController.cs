using System.Collections;
using UnityEngine;

public class PlayerController : Entity, IKillable
{
    // -----------------------------------------------------------------------------
    // Inspector fields
    // -----------------------------------------------------------------------------

    public GameObject explosion;

    [Range(1, 10)]
    public int livesAtStart;

    private int _livesLeft;
    private bool centerIsClear;

    private PlayerMoveManager moveComponent;
    private FireProjectileFromInput fireComponent;

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
            moveComponent.enabled = value;
            fireComponent.enabled = value;
            if (value)
            {
                EventManager.Instance.PlayerSpawned();
            }
            else
            {
                EventManager.Instance.PlayerDespawned();
            }
            Debug.Log("[SetActiveInScene] " + gameObject.name + " : " + value);
        }
    }

    public int Lives
    {
        get { return _livesLeft; }
        private set
        {
            _livesLeft = value;
            EventManager.Instance.PlayerLivesChanged(_livesLeft);
        }
    }



    public override void Awake()
    {
        base.Awake();
        gameObject.name = "Player";
        Lives = livesAtStart;
        moveComponent = GetComponent<PlayerMoveManager>();
        fireComponent = GetComponent<FireProjectileFromInput>();
    }



    void OnEnable()
    {
        EventManager.Instance.OnSpawnSafeZoneClear += HandleCenterIsClear;
        EventManager.Instance.OnPlayerWasHit += Kill;
    }



    void OnDisable()
    {
        EventManager.Instance.OnSpawnSafeZoneClear -= HandleCenterIsClear;
        EventManager.Instance.OnPlayerWasHit -= Kill;
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

        // Notify EventManager
        EventManager.Instance.PlayerDestroyed();
        EventManager.Instance.PlayerLivesChanged(_livesLeft);

        // Check if we should respawn.
        // Otherwise the game is over and we can destroy the player object.
        if (_livesLeft > 0) {
            StartCoroutine(RespawnInSeconds(3));
        }
        else
        {
            EventManager.Instance.PlayerLivesZero();
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
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;
    }
}