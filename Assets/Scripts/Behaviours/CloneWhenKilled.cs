using UnityEngine;
using Random = UnityEngine.Random;

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

    //
    // Properties
    //
    public int Generation {
        get { return _generation; }
        private set
        {
            _generation = value;
            gameObject.name = "Asteroid (Generation " + Generation + ")";
        }
    }

    public CloneWhenKilled SourcePrefab { get; set; }

    private void Awake()
    {
        Generation = 0;
    }

    private void Start()
    {
        if (Generation == 0)
        {
            transform.position = new Vector2(Random.Range(-15f, 15f), Random.Range(3f, 6f));
        }
    }

    public void Kill()
    {
        if (Generation < generationsMax)
        {
            for (int i = 0; i < numberOfClones; i++)
            {
                CloneWhenKilled clone = Instantiate(SourcePrefab, Vector2.zero, Quaternion.identity);
                clone.SourcePrefab = SourcePrefab;
                clone.gameObject.transform.position = transform.position;
                clone.gameObject.transform.SetParent(transform.parent);
                clone.Generation = Generation + 1;
                newScale = Mathf.Pow(scalingFactor, Generation + 1);
                clone.transform.localScale = new Vector3(newScale, newScale, 1);
            }
        }
    }
}
