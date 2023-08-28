using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MountainMaterial")]
public class MountainObstacleMaterial : ScriptableObject
{
    [SerializeField] private  Material _originalMaterial;
    [SerializeField] private Material[] _materialVariations;

    public Material GetMaterial()
    {
        return _originalMaterial;
    }

    public void SetMaterial(Material newMaterial)
    {
        _originalMaterial = newMaterial;
    }

    public void ChangeMaterialValues(int materialIndex)
    {
        _originalMaterial.CopyPropertiesFromMaterial(_materialVariations[materialIndex]);
    }
}
