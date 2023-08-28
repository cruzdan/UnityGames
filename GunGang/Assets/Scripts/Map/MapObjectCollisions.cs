using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectCollisions : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Character":
                SetExplosionOnPosition(other.transform.position);
                other.GetComponent<DeleteMapObject>().ReturnObjectToPool();
                break;
        }
    }

    void SetExplosionOnPosition(Vector3 position)
    {
        ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Explosion, position);
    }
}
