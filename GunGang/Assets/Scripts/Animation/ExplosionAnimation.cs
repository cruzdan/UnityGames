using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    [SerializeField] private Transform[] _explosionObjects;
    [SerializeField] private Vector3 _scaleIncrement;
    [SerializeField] private Vector3 _firstScale;
    private int _totalExplosionObjects;
    int _index;
    private void Start()
    {
        _totalExplosionObjects = _explosionObjects.Length;
    }
    private void Update()
    {
        for(_index = 0; _index < _totalExplosionObjects; _index++)
        {
            _explosionObjects[_index].localScale += _scaleIncrement * Time.deltaTime;
        }
        if(_explosionObjects[0].localScale.x >= 1)
        {
            ResetExplosion();
            ObjectPool.Instance.ReturnObjectToPool(gameObject, ObjectPool.PoolObjectType.Explosion);
        }
    }

    public void ResetExplosion()
    {
        for (_index = 0; _index < _totalExplosionObjects; _index++)
        {
            _explosionObjects[_index].localScale = _firstScale;
        }
    }
}
