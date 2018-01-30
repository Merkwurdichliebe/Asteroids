using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]

public class FireProjectileFromInput : MonoBehaviour {

    public ObjectPool prefabProjectilePool;
    public Transform anchorMainGun;
    private ObjectPool projectilePool;

    private void Awake()
    {
        projectilePool = Instantiate(prefabProjectilePool);
    }

    private void OnEnable()
    {
        InputManager.OnInputFire += Fire;
    }

    private void OnDisable()
    {
        InputManager.OnInputFire -= Fire;
    }

    private void Fire()
    {
        GameObject projectile = projectilePool.GetPooledObject();
        if (projectile != null)
        {
            projectile.transform.position = anchorMainGun.position;
            projectile.transform.rotation = transform.rotation;
            // projectile.tag = "PlayerProjectile";
            projectile.SetActive(true);
        }
    }
}
