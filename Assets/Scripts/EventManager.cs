using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public delegate void EventMessage();
    public static event EventMessage OnAsteroidDestroyed;
    public static event EventMessage OnPlayerDestroyed;
    public static event EventMessage OnLivesEqualsZero;
    public static event EventMessage OnLastAsteroidDestroyed;
    public static event EventMessage OnUFODestroyed;

    public delegate void EventScorePoints(int points);
    public static event EventScorePoints OnScorePoints;

    public delegate void EventUI(int value);
    public static event EventUI OnUIUpdateScore;
    public static event EventUI OnUIUpdateLives;



    // Events for delegate type EventMessage

    public static void MessageAsteroidDestroyed()
    {
        if (OnAsteroidDestroyed != null) OnAsteroidDestroyed();
    }

    public static void MessageLastAsteroidDestroyed()
    {
        if (OnLastAsteroidDestroyed != null) OnLastAsteroidDestroyed();
    }

    public static void MessagePlayerDestroyed()
    {
        if (OnPlayerDestroyed != null) OnPlayerDestroyed();
    }

    public static void MessageLivesEqualsZero()
    {
        if (OnLivesEqualsZero != null) OnLivesEqualsZero();
    }

    public static void MessageUFODestroyed()
    {
        if (OnUFODestroyed != null) OnUFODestroyed();
    }



    // Events for delegate type EventScorePoints

    public static void MessageScorePoints(int points)
    {
        if (OnScorePoints != null) OnScorePoints(points);
    }



    // Events for delegate type EventUI

    public static void UIUpdateScore(int value)
    {
        if (OnUIUpdateScore != null) OnUIUpdateScore(value);
    }

    public static void UIUpdateLives(int value)
    {
        if (OnUIUpdateLives != null) OnUIUpdateLives(value);
    }
}

