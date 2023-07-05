using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCounter : MonoBehaviour
{
    private int _blockCounter = 0;
    [SerializeField] private string _blockTag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_blockTag)) { 
            _blockCounter++;
        }
    }

    public int GetBlockCounter()
    {
        return _blockCounter;
    }
    public void ResetBlockCounter()
    {
        _blockCounter = 0;
    }
}
