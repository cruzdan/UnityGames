using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAdder : MonoBehaviour
{
    [SerializeField] private Transform[] _transforms;
    private GameObject _auxiliarPrefab;
    public void AddPrefabs()
    {
        foreach(Transform objTransform in _transforms)
        {
            _auxiliarPrefab = BlockPool.Instance.GetObjectFromPool();
            _auxiliarPrefab.transform.position = objTransform.position;
            _auxiliarPrefab.transform.SetParent(this.transform);
        }
    }
}
