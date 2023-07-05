using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBlocksTrigger : MonoBehaviour
{
    [SerializeField] private JumpBlocksActivator _jumpBlocks;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _jumpBlocks.Init();
            _jumpBlocks.enabled = true;
        }
    }
}
