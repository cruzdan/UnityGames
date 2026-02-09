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
            CameraShake.Instance.Shake(0.15f, 0.1f);
            MapExplosion.Instance.CreateExplosionOnPosition(other.transform.position);
            SFXManager.Instance.PlaySFX(AudioConstants.Instance.MapObjectDestroyedClip);
            GetComponent<DeleteMapObject>().ReturnObjectToPool();
        }
    }
}
