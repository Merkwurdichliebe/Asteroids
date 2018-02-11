using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private void Awake()
    {
        Count += 1;
    }


    private void OnDestroy()
    {
        Count -= 1;
        if (Count == 0)
        {
            if (OnLastDestroyed != null) { OnLastDestroyed(this); }
        }
    }
}
