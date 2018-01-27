using UnityEngine;

public class EventManager : MonoBehaviour
{
    // My first Event Manager class using delegates.
    // This allows us to decouple classes and avoid having to hold
    // references to a GameManager, a UIManager etc in each class.
    // Objects can simply notify the EventManager class
    // that something has happened. The manager then notifies
    // all the classes which subscribed to a particular message.

    // Declare the delegate type called "EventMessage", which simply
    // defines the signature of the delegate (return void, no arguments).
    public delegate void EventMessage();

    // Declare static events of the type EventMessage (declared above).
    // These are simply containers for methods (or pointers to functions).
    // Any class can subscribe to one of these events using:
    // EventManager.OnAsteroidDestroyed += MethodName
    // This will add MethodName to the list of methods that will be called
    // when the event fires.
    //
    // We could remove the "event" keyword which would then make the containers
    // "multicast delegates" instead. The difference is that with "event"
    // we cannot accidentally unsubscribe all methods with this:
    // EventManager.OnAsteroidDestroyed = MethodName
    // With "event" we can only add or remove a method.
    public static event EventMessage OnAsteroidDestroyed;
    public static event EventMessage OnPlayerDestroyed;
    public static event EventMessage OnLivesEqualsZero;
    public static event EventMessage OnLastAsteroidDestroyed;
    public static event EventMessage OnUFODestroyed;

    // Same as above, but for events which need an argument
    public delegate void EventScorePoints(int points);
    public static event EventScorePoints OnScorePoints;

    public delegate void EventUIMessageRoaming(string text, Vector3 pos);
    public static event EventUIMessageRoaming OnUIMessageRoaming;



    // Static methods which can be called from anywhere, like this:
    // EventManager.MessageAsteroidDestroyed()
    // This will call the event defined above, using:
    // OnAsteroidDestroyed()
    // This, in turn, will call all methods which have subscribed
    // to the event.
    // We are checking for null before calling the event
    // Just to make sure that at least one method has subscribed to it.



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

    public static void UIShowMessageAtWorldPosition(string text, Vector3 pos)
    {
        if (OnUIMessageRoaming != null) OnUIMessageRoaming(text, pos);
    }

}

