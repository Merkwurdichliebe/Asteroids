using UnityEngine;



[RequireComponent(typeof(InputFromKeyboard))]

public class FireProjectile : MonoBehaviour, IFire {

    //
    // Inspector fields 
    //
    public Transform projectileOrigin;

    //
    // Private fields 
    //
    private bool objectPoolExists = true;

    //
    // Properties
    // 
    public bool FiringEnabled { get; set; }

    //
    // Check if the object pool exists and if not display a warning.
    // FIXME how to refer to object pool properly while still not instantiating it every time
    private void Start()
    {
        if (ObjectPool.Instance == null || !ObjectPool.Instance.enabled)
        {
            objectPoolExists = false;
            Debug.LogWarning("[FireProjectile] needs an enabled ObjectPool. Firing is disabled.");
        }
    }


    //
    // Implement IFire interface.
    // We will get a null reference error if we try to get an object
    // from ObjectPool without making sure it exists.
    //
    public void Fire()
    {
        if (objectPoolExists && FiringEnabled)
        {
            // FIXME the string should be generalized
            GameObject projectile = ObjectPool.Instance.GetPooledObject("PlayerProjectile");
            if (projectile != null)
            {
                projectile.transform.position = projectileOrigin.position;
                projectile.transform.rotation = transform.rotation;
                projectile.SetActive(true);
            }
        }
    }
}
