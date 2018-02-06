using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ICanSpawnEntities
{
    // ---------- Inspector fields ----------

    [Header("General Settings")]
    public int secondsBetweenSpawns;
    [Range(0, 1)]
    public float overallSpawnProbability = 1;

    // An list of SpawnableObject objects
    // (cf. struct below)
    // Allows dropping prefabs to spawn
    // and set weight & max values for each
    [Header("Objects to spawn")]
    public SpawnableObject[] spawnableObjects;

    // ---------- Private fields ----------

    private int totalProbabilityWeights;
    private List<SpawnableObject> spawnableObjectsWeighted;
    private Dictionary<string, int> spawnedCount;
    private Coroutine spawnCoroutine;

    private void Awake()
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
            for (int i = 0; i < obj.probabilityWeight; i++)
            {
                // All prefabs should have a component which implements
                // the ISpawnable interface for the NotifyDestroyed()
                // callback to work.
                ISpawnable sp = obj.prefab.GetComponent<ISpawnable>();
                if (sp != null)
                {
                    spawnableObjectsWeighted.Add(obj);
                }
                else
                {
                    Debug.LogError("[Spawner/Awake] Prefab '" + obj.prefab.name +
                                  "' doesn't implement interface 'ISpawnable'.");
                }

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

    // This method gets called by spawned objects when they are destroyed
    // and decreases the count value in the Dictionary.
    // At least one script on the spawned object should implement ISpawnable.
    public void NotifyDestroyed(GameObject obj)
    {
        spawnedCount[obj.name]--;
    }

    private void Start()
    {
        StartSpawning();
    }

    // Other objects might need to ask this script to start spawning,
    // so it's separate from Start()
    public void StartSpawning()
    {
        Debug.Log("[Spawner/StartSpawning]");
        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    // Stop the spawning coroutine
    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
    }

    // Main spawning routine. This continuously spawns the prefabs
    // and is only stopped by StopSpawning().
    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            // Wait before selecting which object to spawn
            yield return new WaitForSeconds(secondsBetweenSpawns);

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
                    o.GetComponent<ISpawnable>().Spawner = this;

                    // Increase the spawn count in the Dictionary.
                    spawnedCount[obj.prefab.name]++;
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
    }
}

// A struct holding an object and a weight for its spawning probability.
// Make it serializable so its fields are visible in the Inspector.
[System.Serializable]
public struct SpawnableObject
{
    public GameObject prefab;

    [Range(0, 10)]
    public int probabilityWeight;
    public int maxSimultaneousInstances;
}