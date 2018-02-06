using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

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
    public Action<bool> OnSpawnSafeZoneClear;
    public Action<int> OnGameScoreChanged;
    public Action OnLivesEqualsZero;

    private void Awake()
    {
        Instance = this;
    }

    // Methods called by various scripts,
    // firing the events required if they have been subscribed to.

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
    public void SpawnSafeZoneIsClear(bool b) { if (OnSpawnSafeZoneClear != null) OnSpawnSafeZoneClear(b); }
    public void GameScoreChanged(int score) { if (OnGameScoreChanged != null) OnGameScoreChanged(score); }
}

