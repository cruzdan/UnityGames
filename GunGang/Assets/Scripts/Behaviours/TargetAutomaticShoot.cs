using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAutomaticShoot : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    private float _timer;
    private Transform _transform;
    private Vector3 _bulletPosition = new(0, -.5f, 1);
    private GameObject _bulletObject;
    private Transform _targetTransform;

    private void Start()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _timer = _bullet.GetTimeToShoot();
    }

    void Update()
    {
        if (CanShoot())
        {
            ShootBullet();
            _timer = _bullet.GetTimeToShoot();
        }
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
    }

    bool CanShoot()
    {
        return _timer <= 0;
    }

    void ShootBullet()
    {
        _bulletObject = ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Bullet,
            _transform.position + _bulletPosition);
        _bulletObject.GetComponent<BulletMovement>().ResetDistanceTraveled();
        _bulletObject.transform.LookAt(_targetTransform);
    }

    public void SetTarget(Transform target)
    {
        _targetTransform = target;
    }

    public Transform GetTarget()
    {
        return _targetTransform;
    }
}
