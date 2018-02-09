using UnityEngine;

public class UFOController : Entity, IKillable, ISpawnable
{
    //
    // Inspector fields 
    //
    public GameObject explosion;

    //
    // This property is injected by Spawner through ISpawnable,
    // so that the UFO can notify its Spawner when it's dead.
    //
    public Spawner Spawner { get; set; }

    //
    // Initialisation 
    //
    public override void Awake()
    {
        base.Awake();
    }

    //
    // FIXME Do we need these three?
    //
    private void OnEnable()
    {
        GameOverManager.OnGameOver += CleanUp;
    }

    private void OnDisable()
    {
        GameOverManager.OnGameOver -= CleanUp;
    }

    void CleanUp()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    //
    // (Required by IKillable)
    // UFO kill sequence.
    //
    public void Kill()
    {
        Debug.Log("[UFOController/Kill]");
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //
    // We need this in order to handle when the UFO leaves the screen.
    // The final event notification is handled in OnDestroy().
    //
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //
    // Notify Spawner that we died. 
    //
    private void OnDestroy()
    {
        Debug.Log("[UFOController/OnDestroy]");
        if (Spawner != null) Spawner.NotifyDestroyed(this.gameObject);
    }
}
