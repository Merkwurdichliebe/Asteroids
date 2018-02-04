using System.Collections.Generic;
using UnityEngine;



public class OldObjectPool : MonoBehaviour {

    public GameObject prefab;
    private List<GameObject> pooledObjects;
    public int poolSize;

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(gameObject.transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }


    /// <summary>
    /// Returns an inactive object in the object pool,
    /// or null if none are found.
    /// </summary>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}