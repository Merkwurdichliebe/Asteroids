using System;
using UnityEngine;

public class SpawnSafeZoneManager : MonoBehaviour {

    //
    // Inspector fields
    //
    public float checkInterval = 0.5f;

    //
    // Private fields
    //
    private Collider2D col;
    private bool isClearThisCheck;
    private bool isClearLastCheck;
    private float timeSinceLastCheck;

    //
    //  Events
    //
    public static Action<bool> OnSpawnSafeZoneCleared;
   
    void Awake()
    {
        col = GetComponent<Collider2D>();
        isClearLastCheck = true;
    }

    private void FixedUpdate()
    {
        if (Time.time > timeSinceLastCheck + checkInterval)
        {
            if (isClearThisCheck && !isClearLastCheck)
            {
                Debug.Log("[SpawnSafeZoneManager] Zone is clear");
                isClearLastCheck = true;
                if (OnSpawnSafeZoneCleared != null) { OnSpawnSafeZoneCleared(true); }
            }
            isClearThisCheck = true;
            timeSinceLastCheck = Time.time;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isClearThisCheck = false;
        if (isClearLastCheck)
        {
            Debug.Log("[SpawnSafeZoneManager] Zone is occupied");
            isClearLastCheck = false;
            if (OnSpawnSafeZoneCleared != null) { OnSpawnSafeZoneCleared(false); }
        }
    }

    //
    // Gizmo definition for the Game window
    //
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, col.GetComponentInChildren<CircleCollider2D>().radius);
    }

}