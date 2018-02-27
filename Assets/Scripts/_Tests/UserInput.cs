using UnityEngine;

public class UserInput : MonoBehaviour {

    private Command fireCommand = new FireCommand ();

    public KeyCode forward = KeyCode.UpArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode fire = KeyCode.Space;

    public Command GetCommand () {
        if (Input.GetKeyDown (fire)) {
            return fireCommand;
        }
        return null;
    }
}
