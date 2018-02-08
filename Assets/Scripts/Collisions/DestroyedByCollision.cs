using UnityEngine;

public class DestroyedByCollision : MonoBehaviour
{
    private IKillable myself;

    [Header("List of tags destroying this object")]
    public string[] tags;

    private void Awake()
    {
        myself = gameObject.GetComponent<IKillable>();
    }

    // Destroy this and other object if colliding with enemy or the player.
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (string item in tags)
        {
            if (collision.gameObject.CompareTag(item))
            {
                myself.Kill();
                break;
            }
        }
    }
}