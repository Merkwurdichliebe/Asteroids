using UnityEngine;

/// <summary>
/// This MonoBehaviour implements IMove in a way which works well for
/// the UFO and powerups: on start, choose a random position off screen.
/// "Moving forward" means moving towards the center of the screen.
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]

public class MoveCrossTheScreenThruCenter : MonoBehaviour, IMove
{
    //
    // Inspector fields
    //
    public float speed = 10;

    //
    // Private fields
    //
    private Rigidbody2D rb;
    private Vector3 newVector;
    private Camera cam;

    //
    // Initialisation
    //
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    //
    // Set a random position on start 
    //
	private void Start()
	{
        // Randomly choose left or right of screen
        // This is defined in viewport coordinates (0 to 1)
        float x = (Random.value < 0.5f) ? 0 : 1;

        // Randomly select a vertical position (0 to 1)
        float y = Random.value;

        // Set the transform
        // Set z to the negative value of the Camera z position
        // (default Camera is at z = -10)
        transform.position = cam.ViewportToWorldPoint(
            new Vector3(x, y, -cam.transform.position.z));
        transform.rotation = Quaternion.identity;

        MoveForward();
	}

    //
    // Implement IMove.
    //
    public void MoveForward()
    {
        // Calculate vector to World center.
        Vector3 vector = (Vector3.zero - transform.position).normalized;
        newVector = vector * speed;

        // Move towards center of screen
        rb.velocity = vector;
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    //
    // Easiest way to calculate a vector pointing right or left
    // from current movement is to exchange x and y
    // while inverting the sign of one of them, regardless of magnitude.
    // This is a bit sloppy, so we normalize the resulting vector
    // in FixedUpdate.
    //
    // FIXME make the UFO turn gradually

    public void TurnLeft()
    {
        newVector = new Vector3(-rb.velocity.y, rb.velocity.x) * speed;
    }

    public void TurnRight()
    {
        newVector = new Vector3(rb.velocity.y, -rb.velocity.x) * speed;
    }

    public void FixedUpdate()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, newVector, Time.deltaTime);
    }
}
