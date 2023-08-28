using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadCollision : MonoBehaviour
{
    [SerializeField] private GameEvent OnPlayerDead;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("CylinderObstacle"))
        {
            SetExplosionOnPosition(transform.position);
            gameObject.SetActive(false);
            OnPlayerDead.TriggerEvent();
        }

        if (hit.gameObject.CompareTag("MapObject"))
        {
            SetExplosionOnPosition(transform.position);
            gameObject.SetActive(false);
            OnPlayerDead.TriggerEvent();
        }
    }

    void SetExplosionOnPosition(Vector3 position)
    {
        ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Explosion, position);
    }
}
