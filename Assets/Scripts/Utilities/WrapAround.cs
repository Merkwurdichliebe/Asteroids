using UnityEngine;

public class WrapAround : MonoBehaviour {

    // LIMIT X & Y POSITIONS SO THAT THEY WRAP AROUND
    // WHEN OBJECT IS GOING OFF SCREEN

    public float xRange;
    public float yRange;

	void Update () {
        // Use Mathf.Repeat modulo operator to limit X & Y positions
        // X position range is from -9 to +9
        // Y position range is from -5 to +5
        // We shift values to a positive range, limit them and then
        // shift them back
        float newX = Mathf.Repeat(transform.position.x + xRange, xRange * 2) - xRange;
        float newY = Mathf.Repeat(transform.position.y + yRange, yRange * 2) - yRange;
        transform.position = new Vector2(newX, newY);
	}
}