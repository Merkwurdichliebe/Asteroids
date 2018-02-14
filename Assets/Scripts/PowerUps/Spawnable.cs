/// <summary>
/// This MonoBehaviour allows the gameobject to be spawned by Spawner
/// and to communicate back to it. Prefabs can't be dropped onto Spawner
/// without this component attached or without another component
/// which derives from it.
/// </summary>

public class Spawnable : Entity {

    public Spawner Spawner { get; set; }

    public void OnDestroy()
    {
        NotifySpawnerOnDestroy();
    }

    public void NotifySpawnerOnDestroy()
    {
        if (Spawner != null) Spawner.NotifyDestroyed(this.gameObject);
    }
}