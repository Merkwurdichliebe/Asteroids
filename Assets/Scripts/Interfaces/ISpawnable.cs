public interface ISpawnableTBE
{
    Spawner Spawner { get; set; }
    void NotifySpawnerOnDestroy();
}