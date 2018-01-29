using System;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static Action OnInputAccelerate;
    public static Action OnInputStop;
    public static Action OnInputTurnRight;
    public static Action OnInputTurnLeft;
    public static Action OnInputFire;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (OnInputAccelerate != null) OnInputAccelerate();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (OnInputStop != null) OnInputStop();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (OnInputTurnLeft != null) OnInputTurnLeft();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (OnInputTurnRight != null) OnInputTurnRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnInputFire != null) OnInputFire();
        }
    }
}
