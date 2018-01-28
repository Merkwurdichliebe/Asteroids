using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public GameObject prefab;
    public int poolSize;
    public List<GameObject> pooledObjects;

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



    public GameObject GetObject()
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
