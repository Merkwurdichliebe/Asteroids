using System;
using UnityEngine;
/// <summary>
/// This MonoBehaviour applies rotation and physics forces
/// based on user input.
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputFromKeyboard))]

public class MovePlayerControlled : MonoBehaviour, IMove
{
    //
    // Inspector fields
    //
    public float rotationMultiplier = 5.0f;
    public float thrustMultiplier = 0.5f;
    public float maximumVelocity = 7;

    //
    // Private fields
    //
    private bool isAccelerating;
    private Rigidbody2D rb;
    private Animator an;

    //
    // Properties
    //
    public float CurrentSpeed { get { return rb.velocity.sqrMagnitude; } }

    //
    // Events
    //
    public static Action OnPlayerAccelerating;
    public static Action OnPlayerStopped;
    public static Action<float> OnPlayerSpeedChanged; 

    //
    // Initialisation 
    //
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        if (rb == null) {
            Debug.LogError("[MovePlayerControlled] Requires Rigidbody2D.");
        }
    }

    //
    // Add force if the object is accelerating. 
    //
    private void FixedUpdate()
    {
        if (isAccelerating)
        {
            rb.AddRelativeForce(Vector2.up * thrustMultiplier, ForceMode2D.Force);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maximumVelocity);
        }
    }

    //
    // IMove implementation 
    //
    public void MoveForward()
    {
        isAccelerating = true;
        if (OnPlayerAccelerating != null) { OnPlayerAccelerating(); }
        an.SetBool("Accelerating", true);
    }

    public void Stop()
    {
        isAccelerating = false;
        if (OnPlayerStopped != null) { OnPlayerStopped(); }
        an.SetBool("Accelerating", false);
    }

    public void TurnLeft()
    {
        rb.transform.Rotate(Vector3.forward * rotationMultiplier);
    }

    public void TurnRight()
    {
        rb.transform.Rotate(Vector3.back * rotationMultiplier);
    }
}
