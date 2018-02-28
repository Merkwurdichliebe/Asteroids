using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This MonoBehaviour checks for collisions with an attached trigger collider.
/// It only sends one message at a time, "clear" or "occupied", and works
/// also when an object is destroyed while touching the trigger or when
/// the SafeZone gameobject is enabled while an object is already
/// touching it. The interval between checks can be set in the Inspector.
/// 
/// This couldn't be achieved solely with OnTriggerEnter and OnTriggerExit.
/// 
/// Purpose: create a safe zone for player spawning.
/// </summary>

[RequireComponent(typeof(Collider2D))]

public class SafeZone : MonoBehaviour {

    //
    // Inspector fields
    //
    public float checkInterval;
    public float maximumIdleTime;

    // maximumIdleTime should be higher than checkInterval,
    // otherwise the safe zone moves immediately on the first check.
    // We'd like to keep it at the starting position of (0,0)
    // if possible.

    //
    // Private fields
    //
    private Collider2D col;
    private bool isClearThisCheck;
    private bool isClearLastCheck;
    private float lastCheckTime;
    private float timeAtEnabled;

    //
    //  Events
    //
    public static Action<bool, Vector2> OnSafeZoneClear;
   
    //
    // Initialisation 
    //
    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        isClearThisCheck = false;
        isClearLastCheck = false;
        timeAtEnabled = Time.time;
        transform.position = Vector2.zero;
        // Debug.Log("[SpawnSafeZoneManager] Zone is enabled");
    }

    private void OnDisable()
    {
        // Debug.Log("[SpawnSafeZoneManager] Zone is disabled");
    }

    //
    // FixedUpdate and OnTriggerStay2D work in tandem. 
    //
    private void FixedUpdate()
    {
        if (Time.time > lastCheckTime + checkInterval)
        {
            // If we are clear this frame and weren't last frame
            // then the zone is clear.
            if (isClearThisCheck && !isClearLastCheck)
            {
                // Debug.Log("[SpawnSafeZoneManager] Zone is clear");
                isClearLastCheck = true;
                if (OnSafeZoneClear != null) { OnSafeZoneClear(true, transform.position); }
                gameObject.SetActive(false);
            }

            isClearThisCheck = true;
            lastCheckTime = Time.time;

            // Try to position the zone somewhere else
            if (Time.time > timeAtEnabled + maximumIdleTime)
            {
                float _posX = Random.Range(-4f, 4f);
                float _posY = Random.Range(-3f, 3f);
                gameObject.transform.position = new Vector2(_posX, _posY);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isClearThisCheck = false;
        if (isClearLastCheck)
        {
            // Debug.Log("[SpawnSafeZoneManager] Zone is occupied");
            isClearLastCheck = false;
            if (OnSafeZoneClear != null) { OnSafeZoneClear(false, transform.position); }
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