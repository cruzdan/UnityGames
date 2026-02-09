using System;
using UnityEngine;

public class TargetAutomaticShoot : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private Bullet _bullet;
    #endregion
    #region Private Variables
    private float _timer;
    private Transform _transform;
    private Vector3 _bulletPosition = new(0, -.5f, 1);
    private GameObject _bulletObject;
    private Transform _targetTransform;
    #endregion
    #region Actions
    public Action<Transform> OnShoot;
    #endregion
    #region Functions
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
            OnShoot?.Invoke(transform);
            SFXManager.Instance.PlaySFX(AudioConstants.Instance.ShootClip);
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
    #endregion
}
