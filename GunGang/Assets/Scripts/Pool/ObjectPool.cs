using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [Serializable]
    struct PoolConfigObject
    {
        public GameObject Prefab;
        public int PrewarmCount;
        public Transform Parent;
    }

    readonly Dictionary<PoolObjectType, Stack<GameObject>> pooledObjects = new Dictionary<PoolObjectType, Stack<GameObject>>();
    [SerializeField] List<PoolConfigObject> PooledPrefabsList;
    GameObject _auxiliarObj;
    public enum PoolObjectType
    {
        Bullet = 0,
        Cylinder = 1,
        Key = 2,
        StaticObstacle1 = 3,
        StaticObstacle2 = 4,
        StaticObstacle3 = 5,
        StaticObstacle4 = 6,
        StaticObstacle5 = 7,
        StaticObstacle6 = 8,
        StaticObstacle7 = 9,
        StaticObstacle8 = 10,
        StaticObstacle9 = 11,
        StaticObstacle10 = 12,
        Cube = 13,
        Cone = 14,
        Triangle = 15,
        Sphere = 16,
        Movable1 = 17,
        Movable2 = 18,
        Movable3 = 19,
        Movable4 = 20,
        Movable5 = 21,
        Movable6 = 22,
        Movable7 = 23,
        MapObjectTrigger = 24,
        Character = 25,
        Explosion = 26,
        Enemy = 27,
        House1 = 28,
        House2 = 29,
        House3 = 30,
        House4 = 31,
    }

    void Start()
    {
        int total = PooledPrefabsList.Count;
        for (int i = 0; i < total; i++)
        {
            CreateObjectsOnPool(i);        
        }
    }

    void CreateObjectsOnPool(int poolIndex)
    {
        CreateStackPoolOnPoolObjectType((PoolObjectType)poolIndex);
        if (HasPoolPrefab(poolIndex))
        {
            CreatePrewarmCountObjectsOnPool(poolIndex);
        }
    }

    void CreateStackPoolOnPoolObjectType(PoolObjectType type)
    {
        pooledObjects[type] = new Stack<GameObject>();
    }

    bool HasPoolPrefab(int poolIndex)
    {
        return PooledPrefabsList[poolIndex].Prefab != null;
    }

    void CreatePrewarmCountObjectsOnPool(int poolIndex)
    {
        int size = PooledPrefabsList[poolIndex].PrewarmCount;
        for (int j = 0; j < size; ++j)
        {
            AddObjectToPool(poolIndex);
        }
    } 

    void AddObjectToPool(int poolIndex)
    {
        _auxiliarObj = Instantiate(PooledPrefabsList[poolIndex].Prefab);
        _auxiliarObj.SetActive(false);
        _auxiliarObj.transform.SetParent(PooledPrefabsList[poolIndex].Parent);
        pooledObjects[(PoolObjectType)poolIndex].Push(_auxiliarObj);
    }

    public GameObject GetObjectFromPool(PoolObjectType poolObjectType)
    {
        int count = pooledObjects[key: poolObjectType].Count;
        while (count > 0)
        {
            _auxiliarObj = pooledObjects[key: poolObjectType].Pop();
            if (_auxiliarObj != null)
            {
                _auxiliarObj.SetActive(true);
                return _auxiliarObj;
            }
            else
            {
                Debug.LogWarning("Found a null object in the pool. Has some code outside the pool destroyed it?");
            }
        }
        AddObjectToPool((int)poolObjectType);
        return GetObjectFromPool(poolObjectType);
    }

    public GameObject GetObjectFromPool(PoolObjectType poolObjectType, Vector3 position)
    {
        
        _auxiliarObj = GetObjectFromPool(poolObjectType);
        _auxiliarObj.transform.position = position;
        
        return _auxiliarObj;
    }

    public GameObject GetObjectFromPool(PoolObjectType poolObjectType, Vector3 position, Quaternion rotation)
    {
        _auxiliarObj = GetObjectFromPool(poolObjectType);
        _auxiliarObj.transform.SetPositionAndRotation(position, rotation);
        return _auxiliarObj;
    }

    public GameObject GetObjectFromPoolWithParent(PoolObjectType poolObjectType, Vector3 position, Transform parent)
    {
        return GetObjectWithPositionAndRotation(poolObjectType, position, Quaternion.identity, parent);
    }

    public GameObject GetObjectFromPoolWithParentAndRotation(PoolObjectType poolObjectType, Vector3 position, 
        Quaternion rotation, Transform parent)
    {
        return GetObjectWithPositionAndRotation(poolObjectType, position, rotation, parent);
    }

    GameObject GetObjectWithPositionAndRotation(PoolObjectType poolObjectType, Vector3 position,
        Quaternion rotation, Transform parent)
    {
        _auxiliarObj = GetObjectFromPool(poolObjectType, position, rotation);
        _auxiliarObj.transform.parent = parent;
        return _auxiliarObj;
    }

    public void ReturnObjectToPool(GameObject objectToDeactivate, PoolObjectType poolObjectType)
    {
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
            pooledObjects[poolObjectType].Push(objectToDeactivate);
        }
    }

    public void ReturnObjectToPoolInPoolParent(GameObject objectToDeactivate, PoolObjectType poolObjectType)
    {
        ReturnObjectToPool(objectToDeactivate, poolObjectType);
        if(objectToDeactivate != null)
        {
            objectToDeactivate.transform.SetParent(PooledPrefabsList[(int)poolObjectType].Parent);
        }
    }

    public void ChangePrefab(PoolObjectType poolObjectType, GameObject newPrefab)
    {
        int index = (int)poolObjectType;
        PoolConfigObject newConfigObject = new PoolConfigObject();
        newConfigObject.Prefab = newPrefab;
        newConfigObject.Parent = PooledPrefabsList[index].Parent;
        newConfigObject.PrewarmCount = PooledPrefabsList[index].PrewarmCount;
        PooledPrefabsList[index] = newConfigObject;
    }
}
