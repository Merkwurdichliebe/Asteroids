using UnityEngine;

/// <summary>
/// This MonoBehaviour casts two rays from the front at a defined angle,
/// and turns right or left in order to avoid the collider
/// they intersect.
/// For this to work we need to turn off (in Physics2D project settings):
/// - Queries Hit Triggers (to avoid reacting to its own firing)
/// - Queries Start in Colliders (to avois reacting to its own collider)
/// </summary>

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
        // For the purpose of raycasting,
        // we assume the object is facing the direction it's moving it
        facingDirection = rb.velocity.normalized;

        // Ray starts from current position,
        // its direction is the direction we're moving in (facingDirection).
        // We rotate it with AngleAxis around the Z axis.
        if (Physics2D.Raycast(transform.position, Quaternion.AngleAxis(detectionAngle, Vector3.forward)
                              * facingDirection, distanceThreshold, objectsToAvoid))
        {
            moveComponent.TurnRight();
        }
        else if (Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-detectionAngle, Vector3.forward)
                              * facingDirection, distanceThreshold, objectsToAvoid))
        {
            moveComponent.TurnLeft();
        }
        else if (Physics2D.Raycast(transform.position, facingDirection, distanceThreshold, objectsToAvoid))
        {
            moveComponent.TurnLeft(); // FIXME turning left when something straight ahead is silly
        }
        //else
        //{
        //    moveComponent.MoveForward();
        //}
    }
}
