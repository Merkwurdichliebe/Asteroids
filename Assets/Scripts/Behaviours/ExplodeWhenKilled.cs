using UnityEngine;

/// <summary>
/// This MonoBehaviour instantiate a selectable explosion prefab
/// at the gameobject's position, optionally destroying it.
/// </summary>

public class ExplodeWhenKilled : MonoBehaviour, IKillable
{
    //
    // Inspector fields 
    //
    public GameObject explosionPrefab;
    public bool destroyObjectOnExplosion;

    //
    // Implementation of IKillable.
    //
    public void Kill()
    {
        Transform obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity).transform;
        obj.SetParent(transform.parent);
        if (destroyObjectOnExplosion)
        {
            Destroy(gameObject);
        }
    }
}