using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRendererToChange;
    [SerializeField] private Material[] _materials;

    public void ChangeMaterialByIndex(int materialIndex)
    {
        _meshRendererToChange.material = _materials[materialIndex];
    }
}
