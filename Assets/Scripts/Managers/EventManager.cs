using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

public static Action<ICanScorePoints> OnEntityKilledByPlayer;

    private void Awake()
    {
        Instance = this;
    }

    // Methods called by various scripts,
    // firing the events required if they have been subscribed to.


    public void EntityKilledByPlayer(ICanScorePoints entity) { if (OnEntityKilledByPlayer != null) OnEntityKilledByPlayer(entity); }
}

