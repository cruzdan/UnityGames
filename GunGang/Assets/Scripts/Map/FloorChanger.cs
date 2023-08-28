using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorChanger : MonoBehaviour
{
    [SerializeField] private MeshRenderer _floorMesh;
    [SerializeField] private Material[] _floorMaterials;
    int _currentFloorIndex;
    int _totalFloors;

    private void Start()
    {
        _totalFloors = _floorMaterials.Length;
        SetFloorMaterialWithIndex(GetRandomFloorIndexWithoutRepetition(-1));
    }

    int GetRandomFloorIndexWithoutRepetition(int floorIndex)
    {
        do
        {
            _currentFloorIndex = Random.Range(0, _totalFloors);
        } while (_currentFloorIndex == floorIndex);
        return _currentFloorIndex;
    }

    void SetFloorMaterialWithIndex(int floorIndex)
    {
        _floorMesh.material = _floorMaterials[floorIndex];
    }

    public void SetRandomFloorMaterialWithoutCurrentRepetition()
    {
        SetFloorMaterialWithIndex(GetRandomFloorIndexWithoutRepetition(_currentFloorIndex));
    }
}
