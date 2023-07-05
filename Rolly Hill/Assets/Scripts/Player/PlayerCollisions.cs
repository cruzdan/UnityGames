using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CubeWall"))
        {
            ExplodeCube(other.gameObject);
        }
    }

    void ExplodeCube(GameObject cube)
    {
        cube.GetComponent<HitExplosion>().AddForce();
        cube.GetComponent<HitExplosion>().SetUseGravity(true);
    }
}
