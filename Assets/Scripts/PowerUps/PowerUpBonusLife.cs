public class PowerUpBonusLife : PowerUp {

    protected override void ApplyEffect()
    {
        recipient.GetComponent<PlayerController>().Lives += 1;
        OnPowerUpGrabbed(gameObject, "+1 LIFE");
    }
}