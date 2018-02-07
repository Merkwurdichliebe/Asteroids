using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputFromKeyboard))]

public class MovePlayerControlled : MonoBehaviour, IMove
{

    public static Action OnPlayerAccelerate;
    public static Action OnPlayerStop;

    private float rotScaler = 5.0f;
    private float thrustScaler = 0.5f;
    private bool isAccelerating;

    private Rigidbody2D rb;

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
        OnPlayerAccelerate(); // FIXME this is only for the audio manager
    }

    public void Stop()
    {
        isAccelerating = false;
        OnPlayerStop(); // FIXME this is only for the audio manager
    }

    public void TurnLeft()
    {
        rb.transform.Rotate(Vector3.forward * rotScaler);
    }

    public void TurnRight()
    {
        rb.transform.Rotate(Vector3.back * rotScaler);
    }

    private void FixedUpdate()
    {
        if (isAccelerating)
        {
            rb.AddRelativeForce(Vector2.up * thrustScaler, ForceMode2D.Force);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 9.9f);
            EventManager.Instance.PlayerSpeedChanged(rb.velocity.magnitude);
        }
    }
}
