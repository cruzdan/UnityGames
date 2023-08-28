using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapObjectActivator : MonoBehaviour
{
    [SerializeField] private MapGenerator _mapManager;
    private SpawnMapObjectInfo _spawnMapObjectInfo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MapObjectTrigger"))
        {
            _spawnMapObjectInfo = other.GetComponent<SpawnMapObjectInfo>();
            CreateMapObjectByMapObjectTrigger(other.gameObject);
        }
    }

    void CreateMapObjectByMapObjectTrigger(GameObject triggeredObject)
    {
        _mapManager.CreateMapObject(_spawnMapObjectInfo.GetSpawnIndex(), _spawnMapObjectInfo.GetObjectIndex());
        ObjectPool.Instance.ReturnObjectToPoolInPoolParent(triggeredObject, ObjectPool.PoolObjectType.MapObjectTrigger);
    }
}
