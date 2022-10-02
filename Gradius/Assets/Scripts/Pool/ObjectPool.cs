using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This class generate a GameObject inactive stack with size MaxPoolSize
 Attach it in a GameObject who will be also the Parent of these objects, set the MaxPoolSize and 
use the methods to get and return objects to the pool 
 */
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int MaxPoolSize;
    private readonly Stack<GameObject> inactiveObjects = new();
    public void SetMaxPoolSize(int size)
    {
        MaxPoolSize = size;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (objectPrefab != null)
        {
            for (int i = 0; i < MaxPoolSize; ++i)
            {
                AddObjectToPool();
            }
        }
    }
    void AddObjectToPool()
    {
        GameObject newObj;
        newObj = Instantiate(objectPrefab) as GameObject;
        newObj.SetActive(false);
        newObj.transform.SetParent(this.transform);
        inactiveObjects.Push(newObj);
    }
    public GameObject GetObjectFromPool()
    {
        int count = inactiveObjects.Count;
        while (count > 0)
        {
            GameObject obj = inactiveObjects.Pop();

            if (obj != null)
            {
                obj.SetActive(true);
                return obj;
            }
            else
            {
                Debug.LogWarning("Found a null object in the pool. Has some code outside the pool destroyed it?");
            }
        }
        AddObjectToPool();
        return GetObjectFromPool();
    }

    public void ReturnObjectToPool(GameObject objectToDeactivate)
    {
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
            inactiveObjects.Push(objectToDeactivate);
        }
    }
}
