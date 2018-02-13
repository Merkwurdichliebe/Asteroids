using System;
using UnityEngine;

/// <summary>
/// This MonoBehaviour uses a static variable to keep count of
/// instances of itself and sends a message when zero it reached.
/// The message passes this class which can then be used to inspect
/// the gameobject's tag (among other things).
/// </summary>

public class KeepInstancesCount : MonoBehaviour {

    GameObject reference;
    
    //
    // Static fields
    //
    public static int Count { get; private set; }

    //
    //  Events
    //
    public static Action<KeepInstancesCount> OnLastDestroyed;

    //
    // Increment count on Awake. 
    //
    private void Awake()
    {
        Count += 1;
        // Debug.Log("[KeepInstancesCount/Awake] " + this.gameObject.name + " " + Count);
    }

    //
    // Decrement count on Destroy. Fire event if zero. 
    //
    private void OnDestroy()
    {
        Count -= 1;
        // Debug.Log("[KeepInstancesCount/OnDestroy] " + this.gameObject.name + " " + Count);
        if (Count == 0)
        {
            if (OnLastDestroyed != null) { OnLastDestroyed(this); }
        }
    }
}
