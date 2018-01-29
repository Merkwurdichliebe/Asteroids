using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MoveToCenter : MonoBehaviour
{
	void Start()
	{
        // Randomly choose left or right of screen
        float x = (Random.value < 0.5f) ? -10 : 10;

        // Randomly select a vertical position
        float y = Random.Range(-6, 6);

        // Set the transform
        transform.position = new Vector2(x, y);
        transform.rotation = Quaternion.identity;

        // Calculate vector to center to screen
        Vector2 vector = Vector3.zero - transform.position;

        // Move towards center of screen
        gameObject.GetComponent<Rigidbody2D>().AddForce(vector * 10);
	}
}
