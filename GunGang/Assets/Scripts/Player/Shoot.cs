using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private Bullet _bullet;
    #endregion
    #region Private Variables
    private float _timer;
    private Transform _transform;
    private Vector3 _bulletPosition = new(-.1f, -.5f, 2.5f);
    private GameObject _bulletObject;
    #endregion
    #region Actions
    public Action<Transform> OnShoot;
    #endregion
    #region Functions
    private void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        if (IsPressingLeftClick())
        {
            if(CanShoot())
            {
                ShootBullet();
                OnShoot?.Invoke(transform);
                SFXManager.Instance.PlaySFX(AudioConstants.Instance.ShootClip);
                _timer = _bullet.GetTimeToShoot();
            }
        }
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
    }

    bool IsPressingLeftClick()
    {
        return Input.GetMouseButton(0);
    }

    bool CanShoot()
    {
        return _timer <= 0;
    }

    void ShootBullet()
    {
        _bulletObject = ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Bullet,
            _transform.position + _bulletPosition, Quaternion.identity);
        _bulletObject.GetComponent<BulletMovement>().ResetDistanceTraveled();
    }
    #endregion
}
