using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    private float _timer;
    private Transform _transform;
    private Vector3 _bulletPosition = new(-.1f, -.5f, 2.5f);
    private GameObject _bulletObject;

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
}
