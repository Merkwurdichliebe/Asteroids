using System;
using UnityEngine;

public class SpawnSafeZoneManager : MonoBehaviour {

    public static Action OnSpawnSafeZoneClear;
    public static Action OnSpawnSafeZoneOccupied;

    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        EnableSafeZone(false);
    }



    private void OnEnable()
    {
        PlayerController.OnPlayerSpawned += HandlePlayerSpawned;
        PlayerController.OnPlayerDestroyed += HandlePlayerDestroyed;
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
        Debug.Log("EnabledSafeZone(): " + enable);
    }



    void OnTriggerExit2D(Collider2D collision)
    {
        OnSpawnSafeZoneClear();
    }



    void OnTriggerStay2D(Collider2D collision)
    {
        OnSpawnSafeZoneOccupied();
    }
}
