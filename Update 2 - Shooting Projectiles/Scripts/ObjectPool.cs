using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    public GameObject poolObject;
    public int poolCount;

    private void Start()
    {
        GameObject obj;
        pooledObjects = new List<GameObject>();

        // instantiates all the objects in the pool and deactivates them
        for (int i = 0; i < poolCount; i++)
        {
            obj = Instantiate(poolObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    // returns an object from the pool
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolCount; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
}
