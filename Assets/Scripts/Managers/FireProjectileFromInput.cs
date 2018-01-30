using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]

public class FireProjectileFromInput : MonoBehaviour {

    public Transform anchorMainGun;

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
        GameObject projectile = ObjectPool.Instance.GetPooledObject("PlayerProjectile");
        if (projectile != null)
        {
            projectile.transform.position = anchorMainGun.position;
            projectile.transform.rotation = transform.rotation;
            projectile.SetActive(true);
        }
    }
}
