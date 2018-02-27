using UnityEngine;

public class FireCommand : Command {

    public override void Execute (ICommand actor) {
        actor.Fire();
    }
}