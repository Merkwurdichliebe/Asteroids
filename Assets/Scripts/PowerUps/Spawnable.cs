/// <summary>
/// A base abstract class for UFO and all PowerUp objects.
/// - A Spawner property (injected by the Spawner when instanced).
/// - Destroy when leaving the screen.
/// - Notify Spawner when destroyed.
/// </summary>

public abstract class Spawnable : Entity {

    public Spawner Spawner { get; set; }

    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        NotifySpawnerOnDestroy();
    }

    public void NotifySpawnerOnDestroy()
    {
        if (Spawner != null) Spawner.NotifyDestroyed(this.gameObject);
    }
}
