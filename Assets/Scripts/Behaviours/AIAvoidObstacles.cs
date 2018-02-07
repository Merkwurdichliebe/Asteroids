using UnityEngine;

// For this to work we need to turn off (in Physics2D project settings):
// Queries Hit Triggers (to avoid turning when firing)
// Queries Start in Colliders (to avoid always triggering a Raycast hit)

[RequireComponent(typeof(Rigidbody2D))]

public class AIAvoidObstacles : MonoBehaviour {

    public float distanceThreshold;
    public float detectionAngle;
    public LayerMask objectsToAvoid;

    private Rigidbody2D rb;
    private IMove moveComponent;
    private Vector2 facingDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveComponent = GetComponent<IMove>();
        if (rb == null)
        {
            Debug.LogError("[MoveCrossTheScreenThruCenter] Requires Rigidbody2D.");
        }
    }

    private void FixedUpdate()
    {
        facingDirection = rb.velocity.normalized;
        // We cast two rays at angle from the front
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-detectionAngle, Vector3.forward) * facingDirection * distanceThreshold, Color.white);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(detectionAngle, Vector3.forward) * facingDirection * distanceThreshold, Color.white);

        // Ray starts from current position, its direction is the direction we're moving in (facingDirection), we rotate it with AngleAxis around the Z axis
        if (Physics2D.Raycast(transform.position, Quaternion.AngleAxis(detectionAngle, Vector3.forward) * facingDirection, distanceThreshold, objectsToAvoid))
        {
            moveComponent.TurnRight();
        }
        if (Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-detectionAngle, Vector3.forward) * facingDirection, distanceThreshold, objectsToAvoid))
        {
            moveComponent.TurnLeft();
        }
    }
}
