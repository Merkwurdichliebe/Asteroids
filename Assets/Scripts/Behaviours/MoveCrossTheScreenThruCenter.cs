using UnityEngine;

/// <summary>
/// This MonoBehaviour implements IMove in a way which works well for
/// the UFO and powerups: on start, choose a random position off screen.
/// "Moving forward" means moving towards the center of the screen.
/// </summary>

public enum State { MovingForward, Turning, Stopped }

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
    private Vector2 newVector;
    private Camera cam;
    private State currentState;

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
        transform.position = cam.ViewportToWorldPoint(new Vector3(x, y, -cam.transform.position.z));
        transform.rotation = Quaternion.identity;

        // Calculate vector to World center.
        Vector3 vector = (Vector3.zero - transform.position).normalized;
        newVector = vector * speed;

        MoveForward();
	}

    //
    // Implement IMove.
    //
    public void MoveForward()
    {
        // Move towards center of screen
        rb.velocity = newVector;

        currentState = State.MovingForward;
        //Debug.Log(string.Format("Moving Forward, new vector: {0}", newVector));
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
        currentState = State.Stopped;
    }

    //
    // Easiest way to calculate a vector pointing right or left
    // from current movement is to exchange x and y
    // while inverting the sign of one of them, regardless of magnitude.
    //

    public void TurnLeft()
    {
        newVector = new Vector3(-rb.velocity.y, rb.velocity.x);
        currentState = State.Turning;
        //Debug.Log(string.Format("Turning Left, new vector: {0}", newVector));
    }

    public void TurnRight()
    {
        newVector = new Vector3(rb.velocity.y, -rb.velocity.x);
        currentState = State.Turning;
    }

    public void FixedUpdate()
    {
        if (currentState == State.Turning)
        {
            // This is essentially the same as Vector3.Slerp but instead
            // the function will ensure that the angular speed
            // and change of magnitude never exceeds maxRadiansDelta
            // and maxMagnitudeDelta.
            rb.velocity = Vector3.RotateTowards(rb.velocity, newVector, 0.05f, 0f);
            if (rb.velocity == newVector)
            {
                currentState = State.MovingForward;
            }
        }
    }
}

// FIXME clean the state machine business