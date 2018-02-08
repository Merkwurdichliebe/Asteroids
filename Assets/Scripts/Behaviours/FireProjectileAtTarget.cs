﻿using System.Collections;
using UnityEngine;

public class FireProjectileAtTarget : MonoBehaviour, IFire
{
    private PlayerController target;
    private Vector3 firingPrecision;
    private Coroutine fireCoroutine;
    public bool FiringEnabled { get; set; }
    public float firingInterval;



    private void Awake()
    {
        UpdateFiringStats();
        AcquireTarget();
        EnableFire();
        Fire();
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerSpawned += EnableFire;
        PlayerController.OnPlayerDespawned += DisableFire;
    }

    private void EnableFire()
    {
        FiringEnabled = true;
    }

    private void DisableFire()
    {
        FiringEnabled = false;
    }


    // Update the UFO's firing precision as the level increases.
    void UpdateFiringStats()
    {
        firingPrecision.x = Random.Range(0, Mathf.Clamp(1 - GameManager.CurrentLevel / 10, 0, 1.0f));
        firingPrecision.y = Random.Range(0, Mathf.Clamp(1 - GameManager.CurrentLevel / 10, 0, 1.0f));
        // firingInterval = Mathf.Clamp(3 - GameManager.CurrentLevel / 10, 1.0f, 3.0f);
        // FIXME add some randomness here and level progression
        Debug.Log("[FireProjectileAtTarget/UpdateStats]");
    }

    // Find the target to shoot at (the player).
    private void AcquireTarget()
    {
        target = FindObjectOfType<PlayerController>(); // FIXME try to find all possible targets
        if (target != null)
        {
            Debug.Log("[FireProjectileAtTarget/AcquireTarget] " + target.gameObject.name);
        }
        else
        {
            Debug.LogWarning("[FireProjectileAtTarget/AcquireTarget] No Player in scene. Fire disabled.");
        }

    }

    public void Fire()
    {
        fireCoroutine = StartCoroutine(FireAtTarget());
    }


    IEnumerator FireAtTarget()
    {
        // Wait for 1 second before starting to fire
        yield return new WaitForSeconds(1);
        while (target != null)
        {
            // Only try to fire if we have an ObjectPool with projectiles.
            if (ObjectPool.Instance != null && FiringEnabled)
            {
                UpdateFiringStats(); // FIXME try not to call this every time or simplify the formula in the method

                // Calculate vector to player.
                Vector2 direction = (target.gameObject.transform.position - transform.position) + firingPrecision;

                // Calculate angle to player.
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Get the projectile from the ObjectPool and set it to active.
                GameObject projectile = ObjectPool.Instance.GetPooledObject("EnemyProjectile");

                // ObjectPoll will return null if no objects left,
                // so we check before firing.
                if (projectile != null)
                {
                    projectile.transform.position = transform.position;
                    projectile.transform.rotation = rotation;
                    projectile.SetActive(true);
                }
            }
   
            // Let the firing interval pass before firing again.
            yield return new WaitForSeconds(firingInterval);
        }
    }



    private void OnDestroy()
    {
        DisableFire();
    }
}