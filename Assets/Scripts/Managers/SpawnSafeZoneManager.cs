using System;
using UnityEngine;

public class SpawnSafeZoneManager : MonoBehaviour {

    private Collider2D col;

    //
    //  Events
    //

    public static Action<bool> OnSpawnSafeZoneCleared; 

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        EnableSafeZone(false);
    }


    private void OnEnable()
    {
        PlayerController.OnPlayerSpawned += HandlePlayerSpawned;
        PlayerController.OnPlayerDestroyed += HandlePlayerDestroyed;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerSpawned -= HandlePlayerSpawned;
        PlayerController.OnPlayerDestroyed -= HandlePlayerDestroyed;
    }



    private void HandlePlayerSpawned()
    {
        EnableSafeZone(false);
    }



    private void HandlePlayerDestroyed()
    {
        EnableSafeZone(true);
    }



    private void EnableSafeZone(bool enable)
    {
        col.enabled = enable;
        Debug.Log("[SpawnSaferZoneManager/EnabledSafeZone]: " + enable);
    }



    void OnTriggerExit2D(Collider2D collision)
    {
        if (OnSpawnSafeZoneCleared != null) { OnSpawnSafeZoneCleared(true); }
    }



    void OnTriggerStay2D(Collider2D collision)
    {
        if (OnSpawnSafeZoneCleared != null) { OnSpawnSafeZoneCleared(false); }
    }

    // FIXME check if it's better to use a non-trigger with isTouching
    // https://docs.unity3d.com/ScriptReference/Collider2D.IsTouching.html


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, col.GetComponentInChildren<CircleCollider2D>().radius);
    }

}