using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxDistance;
    private Transform _transform;
    private float _nextDistanceToTravel = 0;
    private float _distanceTraveled;

    void Start()
    {
        SetCurrentTransform();
    }

    void SetCurrentTransform()
    {
        _transform = this.transform;
    }

    private void FixedUpdate()
    {
        CalculateFollowingZDistance();
        MoveOnZPosition();
        IncrementDistanceTraveled();
        TryReturnBullet();
    }

    void CalculateFollowingZDistance()
    {
        _nextDistanceToTravel = _speed * Time.fixedDeltaTime;
    }

    void MoveOnZPosition()
    {
        _transform.localPosition += _transform.forward * _nextDistanceToTravel;
    }

    void IncrementDistanceTraveled()
    {
        _distanceTraveled += _nextDistanceToTravel;
    }

    void TryReturnBullet()
    {
        if (HasReachedMaxDistance())
        {
			ReturnBulletToPool();
        }
    }

    bool HasReachedMaxDistance()
    {
        return _distanceTraveled >= _maxDistance;
    }

    void ReturnBulletToPool()
    {
        ObjectPool.Instance.ReturnObjectToPool(this.gameObject, ObjectPool.PoolObjectType.Bullet);
    }

    public void SetMaxDistance(float value)
    {
        _maxDistance = value;
    }

    public void ResetDistanceTraveled()
    {
        _distanceTraveled = 0;
    }
}
