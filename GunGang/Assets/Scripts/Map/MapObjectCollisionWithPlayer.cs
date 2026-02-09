using UnityEngine;

public class MapObjectCollisionWithPlayer : MonoBehaviour
{
    [SerializeField] private GameEvent OnPlayerDead;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetExplosionOnPosition(other.transform.position);
            other.gameObject.SetActive(false);
            OnPlayerDead.TriggerEvent();
            SFXManager.Instance.PlaySFX(AudioConstants.Instance.CharacterExplosionClip);
        }
    }

    void SetExplosionOnPosition(Vector3 position)
    {
        ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Explosion, position);
    }
}
