using UnityEngine;

/// <summary>
/// This MonoBehaviour instantiates copies of the gameobject its attached to.
/// When first instantiated (outside of this script), the SourcePrefab
/// property must be set by the instantiating script. This is because it is
/// impossible to have a prefab hold a reference to itself (it holds a 
/// reference to the instance instead, which makes cloning impossible).
/// (cf. https://forum.unity.com/threads/reference-to-prefab-changing-to-clone-self-reference.57312/)
/// 
/// Example (code in instantiating script):
///     CloneWhenKilled asteroid = Instantiate(asteroidPrefab, Vector2.zero, Quaternion.identity);
///     asteroid.SourcePrefab = asteroidPrefab;
/// 
/// Inspector options:
/// - Number of clones to create
/// - Maximum number of generations
/// - Optional scaling factor (make the clones larger or smaller)
/// 
/// This component implements the IKillable interface so that Kill()
/// can be called from anywhere.
/// </summary>

public class CloneWhenKilled : MonoBehaviour, IKillable {

    //
    // Inspector fields
    //
    public int numberOfClones;
    public int generationsMax;
    public float scalingFactor;


    //
    // Private fields 
    //
    private float newScale;
    private int _generation;
    private Rigidbody2D rb;

    //
    // Properties
    //
    public int Generation {
        get { return _generation; }
        private set
        {
            _generation = value;
            gameObject.name = "Asteroid (Generation " + Generation + ")";

            // Make rigidbody mass proportional to the generation.
            // Smaller asteroids should be less heavy.
            if (rb != null)
                rb.mass = rb.mass * 0.5f;
        }
    }

    public CloneWhenKilled SourcePrefab { get; set; }

    //
    // Initialisation
    // Set Generation to zero (used in the property for naming and parenting).
    //
    private void Awake()
    {
        Generation = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    //
    // Implementation of IKillable.
    // Instantiate as many clones as needed.
    //
    public void Kill()
    {
        if (Generation == generationsMax) return;
        
        for (int i = 0; i < numberOfClones; i++)
        {
            CloneWhenKilled clone = Instantiate(SourcePrefab, Vector2.zero, Quaternion.identity, transform.parent);
            clone.SourcePrefab = SourcePrefab;
            clone.gameObject.transform.position = transform.position;
            clone.Generation = Generation + 1;
            newScale = Mathf.Pow(scalingFactor, Generation + 1);
            clone.transform.localScale = new Vector3(newScale, newScale, 1);
        }
    }
}
