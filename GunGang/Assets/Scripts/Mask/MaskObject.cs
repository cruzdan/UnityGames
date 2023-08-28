using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    private void Start()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in meshes)
        {
            foreach(Material material in mesh.materials)
            {
                material.renderQueue = 3002;
            }
        }
    }
}
