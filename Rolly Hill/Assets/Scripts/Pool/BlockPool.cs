using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPool : Singleton<BlockPool>
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int MaxPoolSize;
    private readonly Stack<GameObject> inactiveObjects = new();
    public void SetMaxPoolSize(int size)
    {
        MaxPoolSize = size;
    }
    void Start()
    {
        if (Prefab != null)
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
        newObj = Instantiate(Prefab) as GameObject;
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
            objectToDeactivate.transform.parent = this.transform;
        }
    }

    public void ChangeAllMeshMaterials(Material material)
    {
        foreach(GameObject a in inactiveObjects)
        {
            a.GetComponent<MeshRenderer>().material = material;
        }
    }
}
