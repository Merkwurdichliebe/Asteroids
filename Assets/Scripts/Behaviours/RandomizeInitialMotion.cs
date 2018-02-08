using UnityEngine;

[RequireComponent(typeof(AsteroidController))]

public class RandomizeInitialMotion : MonoBehaviour {

    //
    // Inspector fields
    //
    [Header("Position variations when split")]
    public float randomPositionDelta = 0.5f;

    [Header("Forces applied when spawning")]
    public bool addRandomForce = true;
    public bool addRandomTorque = true;

    [Header("Move faster at each level")]
    public float levelMultiplier = 0.5f;

    //
    // Private fields
    //
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

	void Start () {
        // Set the mass to be proportionate to the asteroid size
        // This makes inter-asteroid collisions more realistic
        rb.mass = 1 / (GetComponent<AsteroidController>().Phase + 1);

        // Add position and rotation variations
        float x = Random.Range(transform.position.x - randomPositionDelta, transform.position.x + randomPositionDelta);
        float y = Random.Range(transform.position.y - randomPositionDelta, transform.position.y + randomPositionDelta);
        float rot = Random.Range(0f, 1f);

        // Set the transform
        transform.position = new Vector2(x, y);
        transform.Rotate(new Vector3(0, 0, rot)); // FIXME should look at EulerAngles for transform

        // Give the asteroid random force and torque
        if (addRandomForce)
        {
            float dirX = Random.Range(-1f, 1f);
            float dirY = Random.Range(-1f, 1f);
            Vector2 randomVector = new Vector2(dirX, dirY) *
                (2 + GameManager.CurrentLevel * levelMultiplier) * rb.mass;
            rb.AddRelativeForce(randomVector, ForceMode2D.Impulse);
        }

        if (addRandomTorque)
        {
            rb.AddTorque(Random.Range(-1 * rb.mass, 1 * rb.mass), ForceMode2D.Impulse);
        }
	}

}
