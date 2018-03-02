using UnityEngine;

public class UserInput : MonoBehaviour {

    private Command commandFire = new CommandFire ();

    public KeyCode forward = KeyCode.UpArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode fire = KeyCode.Space;

    void Update () {
        if (Input.GetKeyDown (fire)) {
            commandFire.Execute (gameObject);
        }
    }
}

// Some comment