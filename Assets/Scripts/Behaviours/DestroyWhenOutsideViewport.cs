using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenOutsideViewport : MonoBehaviour {

    public void OnBecameInvisible()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x > 1.0 || pos.x < 0 || pos.y > 1.0 || pos.y < 0)
        {
            Destroy(gameObject);
        }
    }
}