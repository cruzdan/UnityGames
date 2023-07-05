using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMaterialChanger : MonoBehaviour
{
    [SerializeField] private Material[] _materials;
    public void ChangeBlockMaterialsByIndex(int materialIndex)
    {
        BlockPool.Instance.ChangeAllMeshMaterials(_materials[materialIndex]);
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        int total = blocks.Length;
        for (int i = 0; i < total; i++)
        {
            blocks[i].GetComponent<MeshRenderer>().material = _materials[materialIndex];
        }
    }
}
