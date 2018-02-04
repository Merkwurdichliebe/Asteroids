using System.Collections.Generic;
using UnityEngine;

// Implemented from the tutorial on multiple-objects pool at Ray Wenderlich
// https://www.raywenderlich.com/136091/object-pooling-unity


// -----------------------------------------------------------------------------
// A class to hold the variable for each of the objects to be pooled.
// We are using [System.Serializable] to make it visible in the Editor.
// This is a class for local use and not inherited from MonoBehaviour.
// -----------------------------------------------------------------------------

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;
}

// -----------------------------------------------------------------------------
// The ObjectPool class
// -----------------------------------------------------------------------------

public class ObjectPool : MonoBehaviour {

    // Singleton
    public static ObjectPool Instance;

    // A list of the different items to pool above,
    // and another one for all the pooled objects.
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

    // Parent object for all the pooled objects.
    private Transform parent;

    private void Awake()
    {
        Instance = this;
        parent = new GameObject().transform;
        parent.gameObject.name = "Object Pool";
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.transform.SetParent(parent);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
