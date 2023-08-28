using System.Collections;
using System.Collections.Generic;
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
        }
    }

    void SetExplosionOnPosition(Vector3 position)
    {
        ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Explosion, position);
    }
}
