using UnityEngine;

public class CommandFire : Command {

    public override void Execute (GameObject obj) {
        Debug.Log ("Fire " + obj.name);
    }
}