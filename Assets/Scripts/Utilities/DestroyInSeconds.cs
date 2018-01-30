using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInSeconds : MonoBehaviour {

    public float secondsBeforeDestroyed;

    private void Awake()
    {
        Destroy(gameObject, secondsBeforeDestroyed);
    }
}
