using UnityEngine;

/// <summary>
/// This MonoBehaviour gets the Input.GetKey callbacks
/// and then calls the appropriate methods on another component
/// which implements IMove or IFire.
/// </summary>

public class InputFromKeyboard : MonoBehaviour {

    //
    // Inspector fields 
    //
    [Header("Leave this unchecked to use continuous key input")]
    public bool discreteKeyInput;

    //
    // Private fields
    //
    private IMove moveComponent;
    private IFire[] fireComponents;

    //
    // Get component references and log an error if null. 
    //
    private void Awake()
    {
        moveComponent = GetComponent<IMove>();
        if (moveComponent == null)
        {
            Debug.LogError("InputFromKeyboard needs a Component implementing IMove on: " + gameObject);
        }

        fireComponents = GetComponentsInChildren<IFire>();
        if (fireComponents == null)
        {
            Debug.LogError("InputFromKeyboard needs a Component implementing IFire on: " + gameObject);
        }
    }

    //
    // Call proper input function based on Inspector toggle 
    //
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

    //
    // Get discrete key inputs
    //
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

        // TODO: move fire to child transform and split move and fire controls

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (IFire i in fireComponents)
            {
                i.Fire();
            }
        }
    }

    //
    // Get continuous key inputs
    //
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
            foreach (IFire i in fireComponents)
            {
                i.Fire();
            }
        }
    }
}
