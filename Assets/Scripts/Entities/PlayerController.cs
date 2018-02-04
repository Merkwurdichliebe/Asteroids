using System.Collections;
using UnityEngine;

public class PlayerController : Entity, IKillable
{
    // -----------------------------------------------------------------------------
    // Inspector fields
    // -----------------------------------------------------------------------------

    public GameObject explosion;
    public int lives;

    private int _livesLeft;
    private bool centerIsOccupied;

    private PlayerMoveManager moveComponent;
    private FireProjectileFromInput fireComponent;

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
        Lives = lives;
        moveComponent = GetComponent<PlayerMoveManager>();
        fireComponent = GetComponent<FireProjectileFromInput>();
    }

    public override void SetAlive(bool active)
    {
        base.SetAlive(active);
        moveComponent.enabled = active;
        fireComponent.enabled = active;
        if (active)
        {
            EventManager.Instance.PlayerSpawned();
        }
        else 
        {
            EventManager.Instance.PlayerDespawned();
        }
    }

    void OnEnable()
    {
        EventManager.Instance.OnSpawnSafeZoneClear += CenterIsClear;
        EventManager.Instance.OnSpawnSafeZoneOccupied += CenterIsOccupied;
        EventManager.Instance.OnPlayerWasHit += Kill;
    }

    void OnDisable()
    {
        EventManager.Instance.OnSpawnSafeZoneClear -= CenterIsClear;
        EventManager.Instance.OnSpawnSafeZoneOccupied -= CenterIsOccupied;
        EventManager.Instance.OnPlayerWasHit -= Kill;
    }

    void CenterIsClear() { centerIsOccupied = false; }
    void CenterIsOccupied() { centerIsOccupied = true; }

    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }

    public void ResetPosition()
    {
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
    }

    public void Kill()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        // Hide the player, disable its collider & keyboard input
        SetAlive(false);

        // Reduce 1 life
        Lives -= 1;

        EventManager.Instance.PlayerDestroyed();
        EventManager.Instance.PlayerLivesChanged(_livesLeft);

        if (_livesLeft > 0) {
            StartCoroutine(RespawnInSeconds(3));
        }
        else
        {
            EventManager.Instance.PlayerLivesZero();
            Destroy(gameObject, 3);
        }
    }

    IEnumerator RespawnInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(WaitForCenterClear());
    }

    IEnumerator WaitForCenterClear()
    {
        // Don't do anything while the center is not clear
        while (centerIsOccupied) { yield return null; }
        Spawn();
    }

    public void Spawn()
    {
        SetAlive(true);
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;
    }
}