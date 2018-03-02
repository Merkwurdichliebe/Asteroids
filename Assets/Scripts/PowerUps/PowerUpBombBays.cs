using UnityEngine;

public class PowerUpBombBays : PowerUp {

    protected override void ApplyEffect()
    {
        BombBayController bayController = recipient.GetComponentInChildren<BombBayController>();
        bayController.UpgradeBombBays();
    }
}