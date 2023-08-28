using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDeactivator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "MapObject":
            case "CylinderObstacle":
            case "Character":
                other.GetComponent<DeleteMapObject>().ReturnObjectToPool();
                break;
        }
    }
}
