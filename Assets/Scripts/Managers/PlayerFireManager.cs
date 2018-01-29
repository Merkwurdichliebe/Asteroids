using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireManager : MonoBehaviour {

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
        GameObject projectile = projectilePool.GetObject();
        if (projectile != null)
        {
            projectile.transform.position = anchorMainGun.position;
            projectile.transform.rotation = transform.rotation;
            // projectile.tag = "PlayerProjectile";
            projectile.SetActive(true);
        }
    }
}
