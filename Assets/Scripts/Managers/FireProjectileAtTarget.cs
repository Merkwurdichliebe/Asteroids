using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectileAtTarget : MonoBehaviour, ICanFireAtTarget
{
    private GameObject target;
    private Vector3 firingPrecision;
    private float firingInterval;
    private Coroutine fireCoroutine;

    // Required by ICanFireAtArget
    public GameObject Target { get { return target; } set { target = value; } }

    private void Awake()
    {
        UpdateStats();
    }

    private void OnEnable()
    {
        UFOController.OnUFOSpawned += FireAtTarget;
        UFOController.OnUFODespawned += StopFiring;
    }

    private void OnDisable()
    {
        UFOController.OnUFOSpawned -= FireAtTarget;
        UFOController.OnUFODespawned -= StopFiring;
    }

    void UpdateStats()
    {
        Debug.Log("[FireProjectileAtTarget/UpdateStats]");
        // UFO gets more precise as level increases
        firingPrecision.x = Random.Range(0, Mathf.Clamp(1 - GameManager.level / 10, 0, 1.0f));
        firingPrecision.y = Random.Range(0, Mathf.Clamp(1 - GameManager.level / 10, 0, 1.0f));
        firingInterval = Mathf.Clamp(3 - GameManager.level / 10, 1.0f, 3.0f);
    }

    // Required by ICanFireAtArget
    public void FireAtTarget()
    {
        fireCoroutine = StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            // Calculate vector to player
            Vector2 direction = (target.transform.position - transform.position) + firingPrecision;

            // Calculate angle to player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Fire missile
            GameObject projectile = ObjectPool.Instance.GetPooledObject("EnemyProjectile");
            projectile.transform.position = transform.position;
            projectile.transform.rotation = rotation;
            projectile.SetActive(true);

            yield return new WaitForSeconds(firingInterval);
        }
    }

    private void StopFiring()
    {
        Debug.Log("[FireProjectileAtTarget/StopFiring]");
        StopCoroutine(fireCoroutine);
    }
}
