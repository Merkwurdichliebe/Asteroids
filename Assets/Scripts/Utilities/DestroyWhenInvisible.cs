using UnityEngine;

public class DestroyWhenInvisible : MonoBehaviour {

    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
