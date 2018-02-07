using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MoveCrossTheScreenThruCenter : MonoBehaviour, IMove
{
    Rigidbody2D rb;
    public float speed = 10;

    private Vector3 lastVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("[MoveCrossTheScreenThruCenter] Requires Rigidbody2D.");
        }
    }



	private void Start()
	{
        //
        // Randomly choose left or right of screen
        // This is defined in viewport coordinates (0 to 1)
        //
        float x = (Random.value < 0.5f) ? 0 : 1;

        //
        // Randomly select a vertical position (0 to 1)
        //
        float y = Random.value;

        //
        // Set the transform
        // Set z to the negative value of the Camera z position
        // (default Camera is at z = -10)
        //
        transform.position = Camera.main.ViewportToWorldPoint(
            new Vector3(x, y, -Camera.main.transform.position.z));
        transform.rotation = Quaternion.identity;

        MoveForward();
	}



    public void MoveForward()
    {
        //
        // Calculate vector to World center.
        //
        Vector3 vector = (Vector3.zero - transform.position).normalized;
        lastVector = vector;

        //
        // Move towards center of screen
        //
        rb.AddForce(vector * speed);
    }



    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }



    public void TurnLeft()
    {
        Vector3 newVector = new Vector3(-lastVector.y, lastVector.x);
        rb.AddForce(newVector * speed);
        lastVector = newVector;
    }



    public void TurnRight()
    {
        Vector3 newVector = new Vector3(lastVector.y, -lastVector.x);
        rb.AddForce(newVector * speed);
        lastVector = newVector;
    }

    private void FixedUpdate()
    {
        rb.velocity = lastVector.normalized * speed;
    }
}
