using UnityEngine;

public class UFOController : Entity, IKillable, ISpawnable
{

    public Spawner Spawner { get; set; }

    // -----------------------------------------------------------------------------
    // Inspector fields
    // -----------------------------------------------------------------------------

    // Explosion prefab to be instantiated when destroyed
    public GameObject explosion;

    public override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        GameOverManager.OnGameOver += CleanUp;
    }

    private void OnDisable()
    {
        GameOverManager.OnGameOver -= CleanUp;
    }

    // (Required by IKillable)
    // UFO kill sequence.
    public void Kill()
    {
        Debug.Log("[UFOController/Kill]");
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void CleanUp()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    // We need this in order to handle when the UFO leaves the screen.
    // The final steps are taken in OnDestroy().
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // We decrease the count and fire the event only when destroyed.
    private void OnDestroy()
    {
        Debug.Log("[UFOController/OnDestroy]");
        if (Spawner != null) Spawner.NotifyDestroyed(this.gameObject);
    }


}
