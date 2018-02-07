using System;
using UnityEngine;

public class InputFromKeyboard : MonoBehaviour {

    [Header("Leave this unchecked to use continuous key input")]
    public bool discreteKeyInput;

    private IMove moveComponent;
    private IFire fireComponent;

    private void Awake()
    {
        moveComponent = GetComponent<IMove>();
        if (moveComponent == null)
        {
            Debug.LogError("InputFromKeyboard needs a Component implementing IMove on: " + gameObject);
        }

        fireComponent = GetComponent<IFire>();
        if (fireComponent == null)
        {
            Debug.LogError("InputFromKeyboard needs a Component implementing IFire on: " + gameObject);
        }
    }

    private void Update()
    {
        if (discreteKeyInput)
        {
            GetDiscreteKeyInput();
        }
        else
        {
            GetContinuousKeyInput();
        }
    }

    private void GetDiscreteKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveComponent.MoveForward();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveComponent.Stop();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveComponent.TurnLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveComponent.TurnRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            fireComponent.Fire();
        }
    }

    private void GetContinuousKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveComponent.MoveForward();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            moveComponent.Stop();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveComponent.TurnLeft();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveComponent.TurnRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            fireComponent.Fire();
        }
    }
}
