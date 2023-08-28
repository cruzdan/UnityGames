using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMapObject : MonoBehaviour
{
    [SerializeField] private ObjectPool.PoolObjectType _objectType;

    public ObjectPool.PoolObjectType GetPoolObjectType()
    {
        return _objectType;
    }

    public void ReturnObjectToPool()
    {
        ObjectPool.Instance.ReturnObjectToPoolInPoolParent(this.gameObject, _objectType);
    }
}