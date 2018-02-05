using UnityEngine;

// This firing script needs an object pool of projectiles to fire.
// It expects the singleton class (script) called ObjectPool.

[RequireComponent(typeof(InputFromKeyboard))]

public class FireProjectile : MonoBehaviour, IFire {

    // Get from the Inspector the Transform from which the weapon should fire.
    public Transform anchorMainGun;

    private bool objectPoolExists = true;

    // Check if the object pool exists and if not display a warning.
    private void Start()
    {
        if (ObjectPool.Instance == null || !ObjectPool.Instance.enabled)
        {
            objectPoolExists = false;
            Debug.LogWarning("FireProjectilePool needs an enabled ObjectPool. Firing is disabled.");
        }
    }



    // We will get a null reference error if we try to get an object
    // from ObjectPool without making sure it exists.
    public void Fire()
    {
        if (objectPoolExists)
        {
            GameObject projectile = ObjectPool.Instance.GetPooledObject("PlayerProjectile");
            if (projectile != null)
            {
                projectile.transform.position = anchorMainGun.position;
                projectile.transform.rotation = transform.rotation;
                projectile.SetActive(true);
            }
        }
    }
}
