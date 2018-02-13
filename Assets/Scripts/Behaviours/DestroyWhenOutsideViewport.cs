using UnityEngine;

/// <summary>
/// This MonoBehaviour destroys the gameobject if it became invisible
/// outside the bounds of the camera viewport (i.e. left the screen). 
/// </summary>

public class DestroyWhenOutsideViewport : MonoBehaviour {

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void OnBecameInvisible()
    {
        Vector3 pos = cam.WorldToViewportPoint(transform.position);
        if (pos.x > 1.0 || pos.x < 0 || pos.y > 1.0 || pos.y < 0)
        {
            Destroy(gameObject);
        }
    }
}