using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    //
    // Inspector fields
    //
    [Header("General Settings")]
    public float secondsBetweenSpawns;
    public float overallSpawnProbability = 1;

    // A list of SpawnableObject objects
    // (cf. struct below)
    // Allows dropping prefabs to spawn
    // and set spawn chance & max number for each
    [Header("List of Spawnable prefabs to spawn")]
    public SpawnableObject[] spawnableObjects;

    //
    // Private fields
    //
    private List<SpawnableObject> spawnableObjectsWeighted;
    private Dictionary<string, int> spawnedCount;
    private Coroutine spawnCoroutine;
    private float timeSinceLastSpawn;
    private bool spawnerEnabled;

    //
    // Properties
    //
    public int TotalCount { get; private set; }

    // Check if the SpawnableObject array size has been set
    // then go through initialization. Otherwise log a warning.
    private void Awake()
    {
        if (spawnableObjects.Length > 0)
            Initialize();
        else
            Debug.LogWarning("[Spawner/Awake] Spawner on '" + this.gameObject.name + "' is empty.");
    }

    private void Initialize()
    {
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
            if (obj.prefab != null)
            {
                // Add key value to the count dictionary
                if (!spawnedCount.ContainsKey(obj.prefab.name))
                {
                    spawnedCount[obj.prefab.name] = 0;
                }
            }
        }
    }

    private bool ShouldSpawn()
    {
        return Time.time - timeSinceLastSpawn > secondsBetweenSpawns && this.enabled;
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

    //
    // Spawn objects based on probability.
    // We check if the maximum number of objects of each type has been reached
    // and then compare a random value to its chance to spawn multiplied
    // by the overall spawn probability.
    //
    private void SpawnAnObject()
    {
        foreach (SpawnableObject obj in spawnableObjects)
        {
            if (spawnedCount[obj.prefab.name] < obj.limit && Random.value < overallSpawnProbability * obj.chanceToSpawn)
            {
                // Instantiate the prefab.
                Spawnable o = Instantiate(obj.prefab, this.transform);

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
            }
        }
        timeSinceLastSpawn = Time.time;
    }

    //
    // This method gets called by spawned objects when they are destroyed
    // and decreases the count value in the Dictionary.
    // At least one script on the spawned object should inherit from Spawnable.
    //
    public void NotifyDestroyed(GameObject obj)
    {
        // Debug.Log("[Spawner/NotifyDestroyed] " + obj.name);
        spawnedCount[obj.name]--;
        TotalCount--;
    }
}

//
// A struct holding an object and a spawning probability.
// Make it serializable so its fields are visible in the Inspector.
//
[System.Serializable]
public struct SpawnableObject
{
    public Spawnable prefab;
    public float chanceToSpawn;
    public int limit;
}