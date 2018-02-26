using UnityEngine;

public class FireProjectile : MonoBehaviour, IFire
{
    //
    // Inspector fields 
    //
    [Space]
    public bool enabledAtStart;

    // projectilePrefab is only used for passing the gameobject.name to ObjectPool
    [Space]
    [Header("Prefab used for name only")]
    public GameObject projectilePrefab;
    
    [Header("Projectile settings")]
    public float speed;
    public float lifespan;


    //
    // Private fields 
    //
    private bool objectPoolExists = true;

    //
    // Properties
    //
    public bool IsEnabled { get; set; }

    //
    // Initialisation
    //
    private void Awake()
    {
        IsEnabled = enabledAtStart;    
    }

    //
    // Check if the object pool exists and if not display a warning.
    //
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
    // We set the layer of the projectile to the layer of the
    // gun itself. This allows assigning different projectile types
    // without limiting them to player-only or enemy-only.
    //
    public void Fire()
    {
        if (objectPoolExists && gameObject.activeSelf && IsEnabled)
        {
            GameObject projectile = ObjectPool.Instance.GetPooledObject(projectilePrefab.name);
            if (projectile != null)
            {
                projectile.transform.position = transform.position;
                projectile.transform.rotation = transform.rotation;
                projectile.GetComponent<Projectile>().speed = speed;
                projectile.GetComponent<Projectile>().lifespan = lifespan;
                projectile.gameObject.layer = gameObject.layer;
                projectile.SetActive(true);
            }
        }
    }
}
