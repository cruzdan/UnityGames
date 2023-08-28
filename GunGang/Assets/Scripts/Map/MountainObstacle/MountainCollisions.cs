using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainCollisions : MonoBehaviour
{
    [SerializeField] private Score _score;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            _score.IncrementScore(2);
            ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Explosion, transform.position);
            ObjectPool.Instance.ReturnObjectToPool(other.gameObject, ObjectPool.PoolObjectType.Bullet);
            GetComponent<DeleteMapObject>().ReturnObjectToPool();
        }
    }
}
