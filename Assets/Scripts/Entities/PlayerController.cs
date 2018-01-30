using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : Entity, IKillable
{
    public static Action OnPlayerSpawned;
    public static Action OnPlayerDestroyed;
    public static Action OnPlayerLivesZero;
    public static Action<int> OnPlayerLivesChanged;

    private int livesLeft;
    private bool centerIsOccupied;

    private ParticleSystem ps;

    public int Lives
    {
        get
        {
            return livesLeft;
        }
        set
        {
            livesLeft = value;
            OnPlayerLivesChanged(livesLeft);
        }
    }

    public override void Awake()
    {
        base.Awake();
        gameObject.name = "Player";
        ps = GetComponent<ParticleSystem>();
    }



    void OnEnable()
    {
        SpawnSafeZoneManager.OnSpawnSafeZoneClear += CenterIsClear;
        SpawnSafeZoneManager.OnSpawnSafeZoneOccupied += CenterIsOccupied;
        GameManager.OnLevelStarted += HandleLevelStarted;
        PlayerCollisionManager.OnPlayerWasHit += Kill;
    }

    void OnDisable()
    {
        SpawnSafeZoneManager.OnSpawnSafeZoneClear -= CenterIsClear;
        SpawnSafeZoneManager.OnSpawnSafeZoneOccupied -= CenterIsOccupied;
        GameManager.OnLevelStarted -= HandleLevelStarted;
        PlayerCollisionManager.OnPlayerWasHit -= Kill;
    }

    void HandleLevelStarted()
    {
        gameObject.transform.position = Vector2.zero;
        gameObject.SetActive(true);
        OnPlayerSpawned();
    }


    void CenterIsClear() { centerIsOccupied = false; }
    void CenterIsOccupied() { centerIsOccupied = true; }

    public void Kill()
    {
        GetComponent<FragmentExploder>().Explode(rb.velocity);
        ps.Play();

        // Hide the player, disable its collider & keyboard input
        SetActive(false);

        // Reduce 1 life
        livesLeft -= 1;

        OnPlayerDestroyed();
        OnPlayerLivesChanged(livesLeft);

        if (livesLeft > 0)
        {
            StartCoroutine(Respawn());   
        }
        else
        {
            OnPlayerLivesZero();
            Destroy(gameObject, 3);
        }
    }



    IEnumerator Respawn()
    {
        // Wait 3 seconds before respawn
        yield return new WaitForSeconds(3.0f);

        // Don't do anything while the center is not clear
        while (centerIsOccupied) { yield return null; }

        // Reenable the player when it's clear
        SetActive(true);
        transform.position = Vector2.zero;
        transform.rotation= Quaternion.identity;
        rb.velocity = Vector2.zero;
        OnPlayerSpawned();
    }

    public void Spawn()
    {
        
    }



    public void DeSpawn()
    {

    }
}