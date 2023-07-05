using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingBlock : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private Rigidbody _rb;
    Vector3 _blockPosition;
    bool canCollison = false;
    public void Jump()
    {
        _rb.AddForce(new Vector3(0, force, 0), ForceMode.Impulse);
        canCollison = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (canCollison)
        {
            GenerateBlocks();
            gameObject.SetActive(false);
        }
    }

    void GenerateBlocks()
    {
        GenerateBlock(1.2f, 1);
        GenerateBlock(.4f,1);
        GenerateBlock(-.65f,.5f);
        GenerateBlock(.7f,0);
        GenerateBlock(-.67f,-.5f);
        GenerateBlock(.9f,-.6f);
    }
    void GenerateBlock(float x, float z)
    {
        _blockPosition = transform.position;
        _blockPosition.x += x;
        _blockPosition.z += z;
        BlockPool.Instance.GetObjectFromPool().transform.position = _blockPosition;
    }
    public void ResetVariables()
    {
        canCollison = false;
        _rb.velocity = Vector3.zero;
    }
}
