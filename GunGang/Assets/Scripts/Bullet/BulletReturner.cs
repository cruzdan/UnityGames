using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReturner : MonoBehaviour
{
    [SerializeField] private Transform _bulletParent;

    public void ReturnAllActiveBulletsFromBulletParentToPool()
    {
        for(int i = _bulletParent.childCount - 1; i >= 0; i--)
        {
            if (_bulletParent.GetChild(i).gameObject.activeSelf)
            {
                ObjectPool.Instance.ReturnObjectToPool(_bulletParent.GetChild(i).gameObject, ObjectPool.PoolObjectType.Bullet);
            }
        }
    }
}
