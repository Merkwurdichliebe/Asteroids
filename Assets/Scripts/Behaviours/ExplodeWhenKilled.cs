using UnityEngine;

public class ExplodeWhenKilled : MonoBehaviour, IKillable
{
    //
    // Inspector fields 
    //
    public GameObject explosionPrefab;
    public bool destroyObjectOnExplosion;

    //
    // (Required by IKillable)
    //
    public void Kill()
    {
        Debug.Log("[ExplodeWhenKilled/Kill] " + this.gameObject);
        Transform obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity).transform;
        obj.SetParent(transform.parent);
        if (destroyObjectOnExplosion)
        {
            Destroy(gameObject);
        }
    }
}