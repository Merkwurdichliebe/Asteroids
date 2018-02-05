using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MoveCrossTheScreenThruCenter : MonoBehaviour, IMove
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Requires Rigidbody2D");
        }
    }



	private void Start()
	{
        // Randomly choose left or right of screen
        float x = (Random.value < 0.5f) ? -10 : 10;

        // Randomly select a vertical position
        float y = Random.Range(-6, 6);

        // Set the transform
        transform.position = new Vector3(x, y, 0);
        transform.rotation = Quaternion.identity;

        MoveForward();
	}



    public void MoveForward()
    {
        // Calculate vector to center to screen
        Vector3 vector = Vector3.zero - transform.position;

        // Move towards center of screen
        rb.AddForce(vector * 10);
    }



    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }



    public void TurnLeft()
    {
        // Not implemented
    }



    public void TurnRight()
    {
        // Not implemented
    }
}
