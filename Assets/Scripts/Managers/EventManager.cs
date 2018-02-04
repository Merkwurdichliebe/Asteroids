using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public Action OnAsteroidDestroyed;
    public Action OnPlayerSpawned;
    public Action OnPlayerDespawned;
    public Action OnPlayerDestroyed;
    public Action OnPlayerWasHit;
    public Action<int> OnPlayerLivesChanged;
    public Action OnPlayerLivesZero;
    public Action<float> OnPlayerSpeedChanged;
    public Action<ICanScorePoints> OnEntityKilledByPlayer;
    public Action OnUFODestroyed;
    public Action OnSpawnSafeZoneClear;
    public Action OnSpawnSafeZoneOccupied;
    public Action<int> OnGameScoreChanged;
    public Action OnLivesEqualsZero;
    public Action OnLevelCleared;

    // Static methods which can be called from anywhere, like this:
    // EventManager.MessageAsteroidDestroyed()
    // This will call the event defined above, using:
    // OnAsteroidDestroyed()
    // This, in turn, will call all methods which have subscribed
    // to the event.
    // We are checking for null before calling the event
    // Just to make sure that at least one method has subscribed to it.

    public static EventManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Methods called by the messaging script,
    // firing the events required if subscribed to

    public void AsteroidDestroyed() { if (OnAsteroidDestroyed != null) OnAsteroidDestroyed(); }
    public void PlayerSpawned() { if (OnPlayerSpawned != null) OnPlayerSpawned(); }
    public void PlayerDespawned() { if (OnPlayerDespawned != null) OnPlayerDespawned(); }
    public void PlayerDestroyed() { if (OnPlayerDestroyed != null) OnPlayerDestroyed(); }
    public void PlayerWasHit() { if (OnPlayerWasHit != null) OnPlayerWasHit(); }
    public void PlayerLivesChanged(int lives) { if (OnPlayerLivesChanged != null) OnPlayerLivesChanged(lives); }
    public void PlayerLivesZero() { if (OnPlayerLivesZero != null) OnPlayerLivesZero(); }
    public void PlayerSpeedChanged(float speed) { if (OnPlayerSpeedChanged != null) OnPlayerSpeedChanged(speed); }
    public void EntityKilledByPlayer(ICanScorePoints entity) { if (OnEntityKilledByPlayer != null) OnEntityKilledByPlayer(entity); }
    public void UFODestroyed() { if (OnUFODestroyed != null) OnUFODestroyed(); }
    public void SpawnSafeZoneClear() { if (OnSpawnSafeZoneClear != null) OnSpawnSafeZoneClear(); }
    public void SpawnSafeZoneOccupied() { if (OnSpawnSafeZoneOccupied != null) OnSpawnSafeZoneOccupied(); }
    public void GameScoreChanged(int score) { if (OnGameScoreChanged != null) OnGameScoreChanged(score); }
    public void LevelCleared() { if (OnLevelCleared != null) OnLevelCleared(); }
}

