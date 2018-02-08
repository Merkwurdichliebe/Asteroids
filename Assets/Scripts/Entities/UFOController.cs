using UnityEngine;

public class UFOController : Entity, IKillable, ISpawnable
{

    public Spawner Spawner { get; set; }

    // -----------------------------------------------------------------------------
    // Inspector fields
    // -----------------------------------------------------------------------------

    // Explosion prefab to be instantiated when destroyed
    public GameObject explosion;

    // Base scoring values from which to set the PointValue property
    public int basePointValue;
    public bool displayPointsWhenKilled;

    private int _pointValue;



    public override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        GameOverManager.OnGameOver += CleanUp;
    }



    private void OnDisable()
    {
        GameOverManager.OnGameOver -= CleanUp;
    }



    // (Required by IKillable)
    // UFO kill sequence.
    public void Kill()
    {
        Debug.Log("[UFOController/Kill]");
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }



    void CleanUp()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }



    // We need this in order to handle when the UFO leaves the screen.
    // The final steps are taken in OnDestroy().
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }



    // We decrease the count and fire the event only when destroyed.
    private void OnDestroy()
    {
        Debug.Log("[UFOController/OnDestroy]");
        if (Spawner != null) Spawner.NotifyDestroyed(this.gameObject);
    }



    // Destroy the UFO if it collides with asteroid or the player.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // string objTag = collision.gameObject.tag;
        if (collision.gameObject.CompareTag("Asteroid") || 
            collision.gameObject.CompareTag("Player"))
        {
            Kill();   
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            ScoreController sc = GetComponent<ScoreController>();
            sc.ScorePoints();
            Kill();
        }
    }
}
