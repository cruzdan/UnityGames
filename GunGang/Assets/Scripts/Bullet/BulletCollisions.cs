using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisions : MonoBehaviour
{
    [SerializeField] private Bullet _damage;
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "CylinderObstacle":
                ReduceCylinderLife(other.GetComponent<CylinderObstacle>());
                ObjectPool.Instance.ReturnObjectToPool(this.gameObject, ObjectPool.PoolObjectType.Bullet);
                break;
            case "Enemy":
                other.GetComponent<EnemyBehaviour>().DecrementLife(_damage.GetDamage());
                ObjectPool.Instance.ReturnObjectToPool(this.gameObject, ObjectPool.PoolObjectType.Bullet);
                break;
        }
    }

    void ReduceCylinderLife(CylinderObstacle cylinder)
    {
        cylinder.ReduceCylinderLife(_damage.GetDamage());
    }
}
