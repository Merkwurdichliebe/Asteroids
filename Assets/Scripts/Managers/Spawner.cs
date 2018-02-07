using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ICanSpawnEntities
{
    //
    // Inspector fields
    //

    [Header("General Settings")]
    public float secondsBetweenSpawns;
    [Range(0, 1)]
    public float overallSpawnProbability = 1;

    [Header("Only allow prefabs which implement ISpawnable")]
    public bool spawnableInterfaceIsMandatory;

    // An list of SpawnableObject objects
    // (cf. struct below)
    // Allows dropping prefabs to spawn
    // and set weight & max values for each
    [Header("List of prefabs to spawn")]
    public SpawnableObject[] spawnableObjects;

    //
    // Private fields
    //

    private int totalProbabilityWeights;
    private List<SpawnableObject> spawnableObjectsWeighted;
    private Dictionary<string, int> spawnedCount;
    private Coroutine spawnCoroutine;
    private float timeSinceLastSpawn;

    //
    // Properties
    //

    public int TotalCount { get; private set; }

    // Check if the SpawnableObject array size has been set
    // then go through initialization. Otherwise log a warning.
    private void Awake()
    {
        if (spawnableObjects.Length > 0)
        {
            Initialize();
        }
        else
        {
            Debug.LogWarning("[Spawner/Awake] Spawner on '" + this.gameObject.name + "' is empty.");
        }
    }

    private void Initialize()
    {
        // Initialize a List of SpawnableObject which will contain
        // a number of each equal to its weight. Its the simplest way
        // of implementing weights in a random selection
        spawnableObjectsWeighted = new List<SpawnableObject>();

        // Initialize a Dictionary to count how many objects have been spawned
        // of each SpawnableObject type.
        // The key is the prefab's name, the value is the count.
        spawnedCount = new Dictionary<string, int>();

        // For each spawnable object, add it to the List as many times
        // as its weight value, and set its spawned count to zero.
        foreach (SpawnableObject obj in spawnableObjects)
        {
            // First check if the prefab is actually set,
            // because array size allows the prefab slot to be empty.
            if (obj.prefab != null)
            {
                // Loop through the weight value
                for (int i = 0; i < obj.probabilityWeight; i++)
                {
                    if (spawnableInterfaceIsMandatory)
                    {
                        // All prefabs should have a component which implements
                        // the ISpawnable interface for the NotifyDestroyed()
                        // callback to work.
                        if (obj.prefab.GetComponent<ISpawnable>() == null) 
                        {
                            Debug.LogError("[Spawner/Awake] Prefab '" + obj.prefab.name +
                                "' doesn't implement interface 'ISpawnable'.");
                        }
                    }

                    spawnableObjectsWeighted.Add(obj);

                    // If the Dictionary already has a key with the prefab's name,
                    // log a warning as this will get unpredictable results.
                    if (!spawnedCount.ContainsKey(obj.prefab.name))
                    {
                        spawnedCount[obj.prefab.name] = 0;
                    }
                    else
                    {
                        Debug.LogWarning("[Spawner/Awake] Spawner has prefabs with identical names");
                    }
                }
            }
        }
        if(spawnableObjectsWeighted.Count == 0)
        {
            Debug.LogError("[Spawner] Contains no prefabs.");
        }
    }

    //
    // Reset the spawn timer when enabled.
    //

    private void OnEnable()
    {
        timeSinceLastSpawn = Time.time;
    }

    //
    // Check the spawn timer and call SpawnAnObject when ready.
    //

    private void Update()
    {
        if (Time.time - timeSinceLastSpawn > secondsBetweenSpawns)
        {
            SpawnAnObject();
        }
    }

    private void SpawnAnObject()
    {
        // Compare a random value between 0 and 1
        // to the overallSpawnProbability. 
        if (Random.value < overallSpawnProbability)
        {
            // Get the object to spawn from the weighted List of objects
            SpawnableObject obj = spawnableObjectsWeighted[Random.Range(0, spawnableObjectsWeighted.Count)];

            // If the maximum number hasn't been reached...
            if (spawnedCount[obj.prefab.name] < obj.maxSimultaneousInstances)
            {
                // ...instantiate the prefab.
                GameObject o = Instantiate(obj.prefab);

                // Set the gameObject's name. This is crucial for the
                // NotifyDestroyed() method to work, because the name
                // is the Dictionary key.
                o.name = obj.prefab.name;

                // Set the Spawner property on script implementing ISpawnable
                // to this script, so that it can call back NotifyDestroyed().
                // We need to check for null if we are not enforcing
                // the implementation of ISpawnable.
                // If we *are* enforcing it, an error will have been logged
                // already in Initialize().
                ISpawnable sp = o.GetComponent<ISpawnable>();
                if (sp != null) { sp.Spawner = this; }

                // Increase the spawn count in the Dictionary
                // and in the public property.
                spawnedCount[obj.prefab.name]++;
                TotalCount++;
                timeSinceLastSpawn = Time.time;
                Debug.Log("[Spawner/SpawnCoroutine] " + obj.prefab.name + " spawned");
            }
            else
            {
                Debug.Log("[Spawner/SpawnCoroutine] "
                          + obj.prefab.name + " already at maximum ("
                          + obj.maxSimultaneousInstances + ")");
            }
        }
    }

    // This method gets called by spawned objects when they are destroyed
    // and decreases the count value in the Dictionary.
    // At least one script on the spawned object should implement ISpawnable.
    public void NotifyDestroyed(GameObject obj)
    {
        Debug.Log("[Spawner/NotifyDestroyed] " + obj.name);
        spawnedCount[obj.name]--;
        TotalCount--;
    }
}

// A struct holding an object and a weight for its spawning probability.
// Make it serializable so its fields are visible in the Inspector.
[System.Serializable]
public struct SpawnableObject
{
    public GameObject prefab;

    [Range(1, 10)]
    public int probabilityWeight;
    public int maxSimultaneousInstances;
}