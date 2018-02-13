using UnityEngine;

/// <summary>
/// This MonoBehaviour destroys a gameobject after a set number of seconds.
/// </summary>

public class DestroyInSeconds : MonoBehaviour {

    public float secondsBeforeDestroyed;

    private void Awake()
    {
        Destroy(gameObject, secondsBeforeDestroyed);
    }
}
