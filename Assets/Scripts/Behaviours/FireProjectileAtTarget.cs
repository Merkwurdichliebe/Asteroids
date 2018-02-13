﻿using System.Collections;
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
    public float firingInterval;

    //
    // Private fields
    //
    private GameObject target;
    private Vector3 firingPrecision;
    private Coroutine fireCoroutine;
    private int currentLevel;

    //
    // Properties
    //
    public bool FiringEnabled { get; set; }

    //
    // Initialisation 
    //
    private void Awake()
    {
        // Caching this for later. It might help with cleanup
        // when the app quits, in case GameManager is destroyed
        // before this instance.
        currentLevel = GameManager.CurrentLevel;

        // Shorten the firing interval at each level.
        firingInterval = Mathf.Clamp(firingInterval / currentLevel, 0.2f, 3.0f);

        // Find the Player
        target = GameObject.FindWithTag("Player");

        // Start firing
        EnableFire();
    }

    //
    // Calculate a small vector offset based on the current level.
    // This increases the firing accuracy with each level.
    //
    void SetFiringStats()
    {
        float _precision = Random.Range(0, Mathf.Clamp(1 - currentLevel / 10, 0, 1.0f));
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

        while (target != null)
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
            GameObject projectile = ObjectPool.Instance.GetPooledObject("EnemyProjectile");

            // ObjectPoll will return null if no objects left,
            // so we check before firing.
            if (projectile != null)
            {
                projectile.transform.position = transform.position;
                projectile.transform.rotation = rotation;
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
        PlayerController.OnPlayerSpawned += EnableFire;
        PlayerController.OnPlayerDespawned += DisableFire;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerSpawned -= EnableFire;
        PlayerController.OnPlayerDespawned -= DisableFire;
    }

    //
    // We start and stop firing based on the events above.
    //
    private void EnableFire() {
        FiringEnabled = true;
        Fire();
    }

    private void DisableFire() {
        FiringEnabled = false;
        StopCoroutine(fireCoroutine);
    }
}