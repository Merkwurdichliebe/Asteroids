using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
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
    [Header("List of Spawnable prefabs to spawn")]
    public SpawnableObject[] spawnableObjects;

    //
    // Private fields
    //

    private int totalProbabilityWeights;
    private List<SpawnableObject> spawnableObjectsWeighted;
    private Dictionary<string, int> spawnedCount;
    private Coroutine spawnCoroutine;
    private float timeSinceLastSpawn;
    private bool spawnerEnabled;
    private Transform spawnerParent;

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
        GameObject empty = new GameObject { name = "Spawner objects" };
        spawnerParent = empty.transform;

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
            // Also check if the weight isn't zero, in which case
            // the prefab won't be included in the list at all.
            if (obj.prefab != null && obj.probabilityWeight >0)
            {
                // Loop through the weight value
                for (int i = 0; i < obj.probabilityWeight; i++)
                {
                    spawnableObjectsWeighted.Add(obj);

                    // Add key value to the count dictionary
                    if (!spawnedCount.ContainsKey(obj.prefab.name))
                    {
                        spawnedCount[obj.prefab.name] = 0;
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
        PlayerController.OnPlayerSpawned += EnableSpawner;
        PlayerController.OnPlayerDespawned += DisableSpawner;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerSpawned -= EnableSpawner;
        PlayerController.OnPlayerDespawned += DisableSpawner;
    }

    private void EnableSpawner()
    {
        spawnerEnabled = true;
    }

    private void DisableSpawner()
    {
        spawnerEnabled = false;
    }

    private bool ShouldSpawn()
    {
        return Time.time - timeSinceLastSpawn > secondsBetweenSpawns && spawnerEnabled;
    }

    //
    // Check the spawn timer and call SpawnAnObject when ready.
    //
    private void Update()
    {
        if (ShouldSpawn())
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
                Spawnable o = Instantiate(obj.prefab);
                o.transform.parent = spawnerParent;

                // Set the gameObject's name. This is crucial for the
                // NotifyDestroyed() method to work, because the name
                // is the Dictionary key.
                o.name = obj.prefab.name;

                // Set the Spawner property on the Spawnable to this instance,
                // so that it can call back NotifyDestroyed().
                o.Spawner = this;

                // Increase the spawn count in the Dictionary
                // and in the public property.
                spawnedCount[obj.prefab.name]++;
                TotalCount++;
                Debug.Log("[Spawner/SpawnAnObject] " + obj.prefab.name + " spawned");
            }
            else
            {
                Debug.Log("[Spawner/SpawnAnObject] " + obj.prefab.name + " already at maximum (" + obj.maxSimultaneousInstances + ")");
            }
            timeSinceLastSpawn = Time.time;
        }
    }

    // This method gets called by spawned objects when they are destroyed
    // and decreases the count value in the Dictionary.
    // At least one script on the spawned object should inherit from Spawnable.
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
    public Spawnable prefab;

    [Range(0, 10)]
    public int probabilityWeight;
    public int maxSimultaneousInstances;
}