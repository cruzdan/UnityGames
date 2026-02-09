using UnityEngine;

public class MapObjectColiisionWithBullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            ObjectPool.Instance.ReturnObjectToPool(other.gameObject, ObjectPool.PoolObjectType.Bullet);
            MapExplosion.Instance.CreateExplosionOnPosition(other.transform.position);
        }
    }
}
