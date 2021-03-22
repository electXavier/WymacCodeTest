using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is an utility class to pool objects to minimize CPU usage
 * Can Instantiate objects under single object if null create under Scene
 */

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool; 
    public GameObject objectToPool; // object to be pooled
    public GameObject parentObject; //Check if want to create pool under one object
    public bool shouldExpand;// If pool is not enough expand
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Function to Instantiate objects refer to editor
    public void PoolObjects()
    {
        pooledObjects = new List<GameObject>();

        foreach (ObjectPoolItem item in itemsToPool)
        {
            
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool) as GameObject;
                if (item.parentObject != null)
                    obj.transform.parent = item.parentObject.transform;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    // Grabing pooled object
    public GameObject GetPooledObject(string targetTag)
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy == false && pooledObjects[i].tag == targetTag)
            {
                return pooledObjects[i];
            }
        }
        //if object from the pool is not enough and shouldExpand is true instantiate and add to pool and return
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == targetTag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = Instantiate(item.objectToPool) as GameObject;
                    if (item.parentObject != null)
                        obj.transform.parent = item.parentObject.transform;
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        //if not found return null
        return null;
    }
}
