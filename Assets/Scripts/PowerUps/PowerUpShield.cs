using UnityEngine;

public class PowerUpShield : PowerUp {

    protected override void ApplyEffect()
    {
        // Get the PlayerController component from the collider object
        PlayerController player = recipient.GetComponent<PlayerController>();
        player.shield.gameObject.SetActive(true);
    }
}