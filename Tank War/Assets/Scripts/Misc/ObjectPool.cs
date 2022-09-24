using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    public GameObject[] objectPrefabs;

    public List<GameObject>[] pooledObjects;

    public int[] amountToBuffer;

    List<GameObject> objectInUse = new List<GameObject>();

    //if amountToBuffer is not assigned a value
    public int defaultBufferAmount = 10;
    
    void Start()
    {
        pooledObjects = new List<GameObject>[objectPrefabs.Length];

        int i = 0;
        foreach (GameObject objectPrefab in objectPrefabs)
        {
            pooledObjects[i] = new List<GameObject>();

            int bufferAmount;

            if (i < amountToBuffer.Length) bufferAmount = amountToBuffer[i];
            else
                bufferAmount = defaultBufferAmount;

            for (int n = 0; n < bufferAmount; n++)
            {
                GameObject newObj = Instantiate(objectPrefab) as GameObject;
                newObj.transform.parent = this.transform;
                newObj.name = objectPrefab.name;
                newObj.SetActive(false);
                pooledObjects[i].Add(newObj);
            }

            i++;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RecoverUsedObject();
    }

    public GameObject GetObjectForType(string objectType, bool onlyPooled)
    {
        int index = 0;
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            GameObject prefab = objectPrefabs[i];
            if (prefab.name == objectType)
            {
                index = i;
                break;
            }
        }

        for (int i = 0; i < pooledObjects[index].Count; i++)
        {
            if (pooledObjects[index][i] && !pooledObjects[index][i].activeInHierarchy)
            {
                pooledObjects[index][i].SetActive(true);
                objectInUse.Add(pooledObjects[index][i]);
                return pooledObjects[index][i];
            }

            if (i == pooledObjects[index].Count - 1 && !onlyPooled)
            {
                GameObject temp = Instantiate(objectPrefabs[index]);
                pooledObjects[index].Add(temp);
                objectInUse.Add(temp);
                return temp;
            }
        }

        //If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
        Debug.Log("No object of name " + objectType + " left for use (onlyPooled)");
        return null;
    }

    public GameObject GetObjectPermanently(string objName)
    {
        int index = 0;
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            GameObject prefab = objectPrefabs[i];
            if (prefab.name == objName)
            {
                index = i;
                break;
            }
        }

        //Debug.Log(pooledObjects[index].Count);
        for (int i = 0; i < pooledObjects[index].Count; i++)
        {
            if (pooledObjects[index][i] && !pooledObjects[index][i].activeInHierarchy)
            {
                GameObject result = pooledObjects[index][i];
                //remove from pool
                result.transform.parent = null;
                result.SetActive(true);
                pooledObjects[index].RemoveAt(i);
                return result;
            }

            if (i == pooledObjects[index].Count - 1)
            {
                GameObject temp = Instantiate(objectPrefabs[index]);
                return temp;
            }
        }
        return null;
    }

    void RecoverUsedObject()
    {
        for (int i=0; i<objectInUse.Count; i++)
        {
            if (!objectInUse[i].activeInHierarchy)
            {
                objectInUse[i].transform.parent = null;
                objectInUse[i].transform.parent = this.transform;
                objectInUse.RemoveAt(i);
            }
        }
    }
}
