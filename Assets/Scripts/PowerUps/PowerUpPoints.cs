public class PowerUpPoints : PowerUp {
    
    protected override void ApplyEffect()
    {
        GetComponent<ScoreController>().ScorePoints();
    }
}