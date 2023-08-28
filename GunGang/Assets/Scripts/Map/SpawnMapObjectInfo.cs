using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMapObjectInfo : MonoBehaviour
{
    private int _spawnIndex;
    private int _objectIndex;

    public void SetSpawnIndexAndObjectIndex(int index, int objectIndex)
    {
        _spawnIndex = index;
        _objectIndex = objectIndex;
    }

    public int GetSpawnIndex()
    {
        return _spawnIndex;
    }

    public int GetObjectIndex()
    {
        return _objectIndex;
    }
}
