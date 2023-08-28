using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTiling : MonoBehaviour
{
    [SerializeField] private Material _floorMaterial;
    [SerializeField] private MapGenerator _mapGenerator;

    private void Start()
    {
        SetFloorTilingYAsHalfSizeOfMapZ();
    }

    public void SetFloorTilingYAsHalfSizeOfMapZ()
    {
        _floorMaterial.mainTextureScale = new Vector2(5, _mapGenerator.GetMapZScale() / 2);
    }
}
