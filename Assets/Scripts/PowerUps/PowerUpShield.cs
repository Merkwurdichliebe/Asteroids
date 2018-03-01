using UnityEngine;

public class PowerUpShield : PowerUp {

    protected override void ApplyEffect()
    {
        recipient.GetComponentInChildren<ShieldController>().Activate();
    }
}