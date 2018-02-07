using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputFromKeyboard))]

public class MovePlayerControlled : MonoBehaviour, IMove
{

    public static Action OnPlayerAccelerating;
    public static Action OnPlayerStopped;

    public float rotationMultiplier = 5.0f;
    public float thrustMultiplier = 0.5f;
    private bool isAccelerating;

    private Rigidbody2D rb;

    //
    //  Events
    //

    public static Action<float> OnPlayerSpeedChanged; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("[MovePlayerControlled] Requires Rigidbody2D.");
        }
    }

    public void MoveForward()
    {
        isAccelerating = true;
        if (OnPlayerAccelerating != null) { OnPlayerAccelerating(); }
    }

    public void Stop()
    {
        isAccelerating = false;
        if (OnPlayerStopped != null) { OnPlayerStopped(); }
    }

    public void TurnLeft()
    {
        rb.transform.Rotate(Vector3.forward * rotationMultiplier);
    }

    public void TurnRight()
    {
        rb.transform.Rotate(Vector3.back * rotationMultiplier);
    }

    private void FixedUpdate()
    {
        if (isAccelerating)
        {
            rb.AddRelativeForce(Vector2.up * thrustMultiplier, ForceMode2D.Force);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 9.9f);
            if (OnPlayerSpeedChanged != null) { OnPlayerSpeedChanged(rb.velocity.magnitude); }
        }
    }
}
