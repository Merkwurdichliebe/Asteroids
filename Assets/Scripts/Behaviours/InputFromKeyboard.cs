using System;
using UnityEngine;

public class InputFromKeyboard : MonoBehaviour {

    public static Action OnInputAccelerate;
    public static Action OnInputStop;
    public static Action OnInputTurnRight;
    public static Action OnInputTurnLeft;
    public static Action OnInputFire;

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
