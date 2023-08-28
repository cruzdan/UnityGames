using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletChanger : MonoBehaviour
{
    [SerializeField] private Transform _bulletsParent;
    [SerializeField] private Transform[] _bulletPrefabs;

    public void ChangeAllBulletsForPrefabWithIndex(int bulletIndex)
    {
        DestroyAllBulletChilds();
        AddToAllBulletsNewPartsOfBulletWithIndex(bulletIndex);
        SetBulletWithIndexToPool(bulletIndex);
    }

    void DestroyAllBulletChilds()
    {
        int total = _bulletsParent.childCount;
        for (int i = total - 1; i >= 0; i--)
        {
            DestroyChildsOfTransform(_bulletsParent.GetChild(i));
        }
    }

    void DestroyChildsOfTransform(Transform targetTransform)
    {
        int total = targetTransform.childCount;
        for (int i = total - 1; i >= 0; i--)
        {
            Destroy(targetTransform.GetChild(i).gameObject);
        }
    }

    void AddToAllBulletsNewPartsOfBulletWithIndex(int bulletIndex)
    {
        int totalBulletParts = _bulletPrefabs[bulletIndex].childCount;
        int totalBullets = _bulletsParent.childCount;
        for (int i = 0; i < totalBullets; i++)
        {
            for (int j = 0; j < totalBulletParts; j++)
            {
                Instantiate(_bulletPrefabs[bulletIndex].GetChild(j), _bulletsParent.GetChild(i));
            }
        }
    }

    void SetBulletWithIndexToPool(int bulletIndex)
    {
        ObjectPool.Instance.ChangePrefab(ObjectPool.PoolObjectType.Bullet, _bulletPrefabs[bulletIndex].gameObject);
    }
}
