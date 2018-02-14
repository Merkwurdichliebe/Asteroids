using UnityEngine;

public class DestroyedByCollision : MonoBehaviour
{
    [Header("List of tags destroying this object")]
    public string[] hostileTags;
    public LayerMask layerMask;

    // We need to send Kill() to all components implementing IKillable
    // (explode when killed, destroy when colliding...)
    private IKillable[] killables;

    private void Awake()
    {
        killables = gameObject.GetComponents<IKillable>();
    }

    // Kill if colliding with object in selected layer
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & layerMask) != 0)
        {
            for (int i = 0; i < killables.Length; i++)
            {
                killables[i].Kill();
            }
        }
    }
}