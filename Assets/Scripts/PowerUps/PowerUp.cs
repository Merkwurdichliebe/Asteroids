using UnityEngine;

/// <summary>
/// This Spawnable:Entity:MonoBehaviour abstract class is the parent
/// for all PowerUp classes. Child classes only need to define ApplyEffect().
/// </summary>

public abstract class PowerUp : Spawnable {

    //
    // Inspector fields 
    //
    public GameObject pickUpFX;

    //
    // Private fields
    //
    protected GameObject recipient;
    protected abstract void ApplyEffect();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(pickUpFX, transform.position, Quaternion.identity);
            recipient = collision.gameObject;
            ApplyEffect();
            Destroy(gameObject);
        }
    }
}
