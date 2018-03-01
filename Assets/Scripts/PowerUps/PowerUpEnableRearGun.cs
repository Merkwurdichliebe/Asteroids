using UnityEngine;

public class PowerUpEnableRearGun : PowerUp {

    protected override void ApplyEffect()
    {
        // Get the PlayerController component from the collider object
        BombBayController controller = recipient.GetComponentInChildren<BombBayController>();

        // Go through the Weapons list and find the Rear Gun, then enable it
        // foreach (GameObject weapon in controller.Bays)
        // {
        //     if (weapon.name == "Rear Gun")
        //     {
        //         weapon.GetComponent<IFire>().IsEnabled = true;
        //         weapon.SetActive(true);
        //         OnPowerUpGrabbed(gameObject, "BOMBS");
        //     }
        // }

        controller.EnableBombBay();
    }
}