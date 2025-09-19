using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Object Pool para objetos normales (offline, sin Netcode).
/// </summary>
public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    public static ObjectPool Singleton { get { return _instance; } }

    [SerializeField]
    List<PoolConfigObject> PooledPrefabsList;

    HashSet<GameObject> prefabs = new HashSet<GameObject>();
    Dictionary<GameObject, Queue<GameObject>> pooledObjects = new Dictionary<GameObject, Queue<GameObject>>();

    private bool m_HasInitialized = false;
    GameObject auxiliarObject;

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        InitializePools();
    }

    public void OnValidate()
    {
        for (var i = 0; i < PooledPrefabsList.Count; i++)
        {
            var prefab = PooledPrefabsList[i].Prefab;
            if (prefab != null)
            {
                Assert.IsFalse(prefabs.Contains(prefab), $"Prefab {prefab.name} is already registered in the pool.");
            }
        }
    }

    /// <summary>
    /// Obtiene una instancia del prefab desde el pool.
    /// </summary>
    public GameObject GetObject(GameObject prefab)
    {
        return GetObjectInternal(prefab, Vector3.zero, Quaternion.identity);
    }

    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return GetObjectInternal(prefab, position, rotation);
    }

    GameObject GetPrefabFromPrefabs(string name)
    {
        foreach (var a in prefabs)
        {
            if (a.name.Equals(name))
            {
                return a;
            }
        }
        Debug.LogError("Prefab is not contained in prefabs: " + name);
        return null;
    }

    public GameObject GetObject(string name)
    {
        return GetObject(name, Vector3.zero, Quaternion.identity);
    }

    public GameObject GetObject(string name, Vector3 position, Quaternion rotation)
    {
        auxiliarObject = GetPrefabFromPrefabs(name);
        if (auxiliarObject != null)
        {
            return GetObjectInternal(auxiliarObject, position, rotation);
        }
        return null;
    }

    /// <summary>
    /// Devuelve un objeto al pool.
    /// </summary>
    public void ReturnObject(GameObject obj, GameObject prefab)
    {
        obj.SetActive(false);
        pooledObjects[prefab].Enqueue(obj);
    }

    public void ReturnObject(GameObject obj, string name)
    {
        ReturnObject(obj, GetPrefabFromPrefabs(name));
    }

    /// <summary>
    /// Añade un prefab al pool.
    /// </summary>
    public void AddPrefab(GameObject prefab, int prewarmCount = 0)
    {
        Assert.IsFalse(prefabs.Contains(prefab), $"Prefab {prefab.name} is already registered in the pool.");
        RegisterPrefabInternal(prefab, prewarmCount);
    }

    private void RegisterPrefabInternal(GameObject prefab, int prewarmCount)
    {
        prefabs.Add(prefab);
        var prefabQueue = new Queue<GameObject>();
        pooledObjects[prefab] = prefabQueue;
        for (int i = 0; i < prewarmCount; i++)
        {
            var go = CreateInstance(prefab);
            ReturnObject(go, prefab);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private GameObject CreateInstance(GameObject prefab)
    {
        foreach (var pooledPrefab in PooledPrefabsList)
        {
            if (pooledPrefab.Prefab == prefab && pooledPrefab.parent != null)
            {
                return Instantiate(prefab, pooledPrefab.parent);
            }
        }
        return Instantiate(prefab);
    }

    public Transform GetPrefabParent(string prefabName)
    {
        foreach (var pooledPrefab in PooledPrefabsList)
        {
            if (pooledPrefab.Prefab.name == prefabName)
            {
                return pooledPrefab.parent;
            }
        }
        return null;
    }

    private GameObject GetObjectInternal(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (pooledObjects[prefab].Count > 0)
        {
            obj = pooledObjects[prefab].Dequeue();
        }
        else
        {
            obj = CreateInstance(prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        obj.transform.SetParent(GetPrefabParent(prefab.name));
        return obj;
    }

    /// <summary>
    /// Inicializa todos los pools.
    /// </summary>
    public void InitializePools()
    {
        if (m_HasInitialized) return;
        foreach (var configObject in PooledPrefabsList)
        {
            RegisterPrefabInternal(configObject.Prefab, configObject.PrewarmCount);
        }
        m_HasInitialized = true;
    }

    /// <summary>
    /// Limpia todos los pools.
    /// </summary>
    public void ClearPool()
    {
        pooledObjects.Clear();
    }
}
