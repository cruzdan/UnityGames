using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPathTrigger : MonoBehaviour
{
    [SerializeField] private FallingBlockSPathCreator _sPath;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _sPath.ResetSPathValues();
            _sPath.enabled = true;
        }
    }
}
