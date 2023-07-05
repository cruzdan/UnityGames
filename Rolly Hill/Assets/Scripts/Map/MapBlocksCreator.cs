using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlocksCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] _paths;
    [SerializeField] private BlockAdder[] _blockAdders;
    [SerializeField] private PathCheckTrigger[] _pathChecks;
    [SerializeField] private int _totalBlocks;
    public void CreateMapBlocks()
    {
        CreateAllPathsShapes();
        if (MapHasBlockAdders())
        {
            ActivateBlockAdders();
        }
    }
    void CreateAllPathsShapes()
    {
        int totalPaths = _paths.Length;
        for (int i = 0; i < totalPaths; i++)
        {
            CreateShapesOfPath(i);
        }
    }

    void CreateShapesOfPath(int pathIndex)
    {
        int totalChilds = _paths[pathIndex].transform.childCount;
        for (int j = 0; j < totalChilds; j++)
        {
            _paths[pathIndex].transform.GetChild(j).GetComponent<Shape>().CreateShapeWithBlocks();
        }
    }

    bool MapHasBlockAdders()
    {
        return _blockAdders != null;
    }

    void ActivateBlockAdders()
    {
        int totalAdders;
        totalAdders = _blockAdders.Length;
        for (int i = 0; i < totalAdders; i++)
        {
            _blockAdders[i].AddPrefabs();
        }
    }

    public void AddCounterToPathChecks(BlockCounter counter)
    {
        int total = _pathChecks.Length;
        for (int i = 0; i < total; i++)
        {
            _pathChecks[i].SetBlockCounter(counter);
        }
    }

    public int GetTotalBlocks()
    {
        return _totalBlocks;
    }
}
