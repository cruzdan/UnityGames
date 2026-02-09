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
                MapExplosion.Instance.CreateExplosionOnPosition(transform.position);
                break;
            case "Enemy":
                MapExplosion.Instance.CreateExplosionOnPosition(transform.position);
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
