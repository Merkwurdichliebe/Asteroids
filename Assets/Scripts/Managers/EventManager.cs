using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;


    private void Awake()
    {
        Instance = this;
    }
}

