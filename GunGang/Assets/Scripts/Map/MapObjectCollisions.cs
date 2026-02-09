using UnityEngine;

public class MapObjectCollisions : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Character":
                SetExplosionOnPosition(other.transform.position);
                CameraShake.Instance.Shake(0.15f, 0.2f);
                other.GetComponent<DeleteMapObject>().ReturnObjectToPool();
                UIVignette.Instance.ShowVignetteWithAnimation(0.5f);
                SFXManager.Instance.PlaySFX(AudioConstants.Instance.CharacterExplosionClip);
                break;
        }
    }

    void SetExplosionOnPosition(Vector3 position)
    {
        ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Explosion, position);
    }
}
