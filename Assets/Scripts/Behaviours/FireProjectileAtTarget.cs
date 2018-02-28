using System.Collections;
using UnityEngine;

/// <summary>
/// This MonoBehaviour fires projectiles repeatedly at an object
/// tagged with "Player", with random precision based on current level.
/// </summary>

public class FireProjectileAtTarget : MonoBehaviour, IFire
{
    //
    // Inspector fields
    //

    // projectilePrefab is only used for passing the gameobject.name to ObjectPool
    public GameObject projectilePrefab;
    public float firingInterval = 3.0f;

    //
    // Private fields
    //
    private GameObject target;
    private Vector3 firingPrecision;
    private Coroutine fireCoroutine;

    //
    // Properties
    //
    public bool IsEnabled { get; set; }

    //
    // Initialisation 
    //
    private void Awake()
    {
        // Shorten the firing interval at each level.
        firingInterval = Mathf.Clamp(firingInterval / GameManager.CurrentLevel * 2, 0.2f, firingInterval);

        // Find the Player
        target = GameObject.FindWithTag("Player");

        // Start firing
        EnableFire(target);
    }

    //
    // Calculate a small vector offset based on the current level.
    // This increases the firing accuracy with each level.
    //
    void SetFiringStats()
    {
        float _precision = Random.Range(0, Mathf.Clamp(1 - GameManager.CurrentLevel / 10, 0, 1.0f));
        firingPrecision.x = _precision;
        firingPrecision.y = _precision;
    }

    //
    // IFire implementation
    // Start the firing coroutine.
    //
    public void Fire()
    {
        // Only try to fire if we have an ObjectPool with projectiles.
        if (ObjectPool.Instance != null)
        {
            fireCoroutine = StartCoroutine(FireAtTarget());
        }
    }

    //
    // Fire repeatedly at the target.
    //
    IEnumerator FireAtTarget()
    {
        // Wait for 1 second before starting to fire
        yield return new WaitForSeconds(1);

        while (target != null && target.activeSelf)
        {
            // Update firingprecision at each shot,
            // otherwise the offset is the same every time.
            SetFiringStats();

            // Calculate vector to player.
            Vector2 direction = target.transform.position - transform.position + firingPrecision;

            // Calculate angle to player.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Get the projectile from the ObjectPool.
            GameObject projectile = ObjectPool.Instance.GetPooledObject(projectilePrefab.name);

            // ObjectPoll will return null if no objects left,
            // so we check before firing.
            if (projectile != null)
            {
                projectile.transform.position = transform.position;
                projectile.transform.rotation = rotation;
                projectile.gameObject.layer = gameObject.layer;
                projectile.SetActive(true);
            }
               
            // Let the firing interval pass before firing again.
            yield return new WaitForSeconds(firingInterval);
        }
    }

    //
    // Event listeners 
    //
    private void OnEnable()
    {
        PlayerManager.OnPlayerEnabled += EnableFire;
        PlayerManager.OnPlayerDisabled += DisableFire;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerEnabled -= EnableFire;
        PlayerManager.OnPlayerDisabled -= DisableFire;
    }

    //
    // We start and stop firing based on the events above.
    //
    private void EnableFire(GameObject player)
    {
        target = player;
        Fire();
    }

    private void DisableFire(GameObject player)
    {
        StopCoroutine(fireCoroutine);
    }
}