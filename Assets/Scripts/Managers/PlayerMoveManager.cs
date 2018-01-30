using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMoveManager : MonoBehaviour {

    public static Action<float> OnPlayerSpeedChanged;
    public static Action OnPlayerAccelerate;
    public static Action OnPlayerStop;

    private float rotScaler = 5.0f;
    private float thrustScaler = 0.5f;
    private bool isAccelerating;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        Debug.Log("[PlayerMoveManager/OnEnable]");
        PlayerController.OnPlayerSpawned += EnableInput;
        PlayerController.OnPlayerDestroyed += DisableInput;
    }

    void OnDisable()
    {
        Debug.Log("[PlayerMoveManager/OnDisable]");
        PlayerController.OnPlayerSpawned -= EnableInput;
        PlayerController.OnPlayerDestroyed -= DisableInput;
    }

    void EnableInput()
    {
        Debug.Log("[PlayerMoveManager/EnableInput]");
        InputManager.OnInputAccelerate += HandleInputAccelerate;
        InputManager.OnInputStop += HandleInputStop;
        InputManager.OnInputTurnLeft += HandleInputTurnLeft;
        InputManager.OnInputTurnRight += HandleInputTurnRight;
    }

    void DisableInput()
    {
        Debug.Log("[PlayerMoveManager/DisableInput]");
        HandleInputStop(); // FIXME Clean this
        InputManager.OnInputAccelerate -= HandleInputAccelerate;
        InputManager.OnInputStop -= HandleInputStop;
        InputManager.OnInputTurnLeft -= HandleInputTurnLeft;
        InputManager.OnInputTurnRight -= HandleInputTurnRight;
    }

    void HandleInputAccelerate()
    {
        isAccelerating = true;
        OnPlayerAccelerate();
    }

    void HandleInputStop()
    {
        isAccelerating = false;
        OnPlayerStop();
    }

    void HandleInputTurnLeft()
    {
        rb.transform.Rotate(Vector3.forward * rotScaler);
    }

    void HandleInputTurnRight()
    {
        rb.transform.Rotate(Vector3.back * rotScaler);
    }

    void FixedUpdate()
    {
        if (isAccelerating)
        {
            rb.AddRelativeForce(Vector2.up * thrustScaler, ForceMode2D.Force);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 9.9f);
            // velocity = rb.velocity; // Cached for velocity at impact
            OnPlayerSpeedChanged(rb.velocity.magnitude);
        }
    }

}
