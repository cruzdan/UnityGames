using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderChanger : MonoBehaviour
{
    [SerializeField] private Transform _cylindersParent;
    [SerializeField] private Transform _mapObjectsParent;
    [SerializeField] private Transform[] _cylinderPrefabs;

    public void ChangeAllCylindersForPrefabWithIndex(int cylinderIndex)
    {
        DestroyAllCylinderChilds();
        AddToAllCylindersNewPartsOfCylinderWithIndex(cylinderIndex);
    }

    void DestroyAllCylinderChilds()
    {
        DestroyAllCylinderChildsInCylindersParent();
        DestroyAllSpawnedCylinderChilds();
    }

    void DestroyAllCylinderChildsInCylindersParent()
    {
        int total = _cylindersParent.childCount;
        for (int i = total - 1; i >= 0; i--)
        {
            DestroyChildsOfTransform(_cylindersParent.GetChild(i));
        }
    }

    void DestroyChildsOfTransform(Transform targetTransform)
    {
        int total = targetTransform.childCount;
        for (int i = total - 1; i >= 0; i--)
        {
            Destroy(targetTransform.GetChild(i).gameObject);
        }
    }

    void DestroyAllSpawnedCylinderChilds()
    {
        int total = _mapObjectsParent.childCount;
        Transform mapChild;
        for (int i = 0; i < total; i++)
        {
            mapChild = _mapObjectsParent.GetChild(i);
            if (mapChild.CompareTag("CylinderObstacle"))
            {
                DestroyChildsOfTransform(mapChild);
            }
        }
    }

    void AddToAllCylindersNewPartsOfCylinderWithIndex(int cylinderIndex)
    {
        AddPrefabPartsToCylindersInCylindersParentWithIndex(cylinderIndex);
        AddPrefabPartsToSpawnedCylindersWithIndex(cylinderIndex);
    }

    void AddPrefabPartsToCylindersInCylindersParentWithIndex(int cylinderIndex)
    {
        int totalCylinderParts = _cylinderPrefabs[cylinderIndex].childCount;
        int totalCylinders = _cylindersParent.childCount;
        for (int i = 0; i < totalCylinders; i++)
        {
            AddPartsToTransformWithIndex(cylinderIndex, _cylindersParent.GetChild(i));
        }
    }

    void AddPrefabPartsToSpawnedCylindersWithIndex(int cylinderIndex)
    {
        int totalCylinderParts = _cylinderPrefabs[cylinderIndex].childCount;
        int totalMapObjects = _mapObjectsParent.childCount;
        Transform mapChild;
        for (int i = 0; i < totalMapObjects; i++)
        {
            mapChild = _mapObjectsParent.GetChild(i);
            if (mapChild.CompareTag("CylinderObstacle"))
            {
                AddPartsToTransformWithIndex(cylinderIndex, mapChild);
            }
        }
    }

    void AddPartsToTransformWithIndex(int cylinderIndex, Transform targetTransform)
    {
        int totalCylinderParts = _cylinderPrefabs[cylinderIndex].childCount;
        for (int j = 0; j < totalCylinderParts; j++)
        {
            Instantiate(_cylinderPrefabs[cylinderIndex].GetChild(j), targetTransform);
        }
    }
}
