// We're using Linq for array.Contains()
using System.Linq;
using UnityEngine;

public class DestroyedByCollision : MonoBehaviour
{
    private IKillable myself;

    [Header("List of tags destroying this object")]
    public string[] hostileTags;
    private IKillable[] killables;

    private void Awake()
    {
        killables = gameObject.GetComponents<IKillable>();
    }

    // Destroy this and other object if colliding with enemy or the player.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hostileTags.Contains(collision.gameObject.tag))
        {
            foreach (IKillable killable in killables)
            {
                killable.Kill();
            }
        }
    }
}