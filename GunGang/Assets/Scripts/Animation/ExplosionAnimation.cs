using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    [SerializeField] private Transform[] _explosionObjects;
    [SerializeField] private Vector3 _scaleIncrement;
    [SerializeField] private Vector3 _firstScale;
    [SerializeField] private ObjectPool.PoolObjectType explosionPool = ObjectPool.PoolObjectType.Explosion;
    [SerializeField] private bool changeParticlePositions;
    private int _totalExplosionObjects;
    private int _index;
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
            ObjectPool.Instance.ReturnObjectToPool(gameObject, explosionPool);
        }
    }

    public void ResetExplosion()
    {
        for (_index = 0; _index < _totalExplosionObjects; _index++)
        {
            _explosionObjects[_index].localScale = _firstScale;
        }
    }

    public void ChangeParticlePositions()
    {
        for (_index = 0; _index < _totalExplosionObjects; _index++)
        {
            _explosionObjects[_index].position = transform.position + Random.insideUnitSphere * 0.5f;
        }
    }

    private void OnEnable()
    {
        if (changeParticlePositions)
        {
            ChangeParticlePositions();
        }
    }
}
