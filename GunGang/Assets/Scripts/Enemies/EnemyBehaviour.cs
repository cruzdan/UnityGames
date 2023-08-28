using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private FollowTargetPosition _followTargetPosition;
    [SerializeField] private Score _score;
    [SerializeField] private int _life;
    private event Action OnEnemyDead;

    public void ClearOnEnemyDead()
    {
        OnEnemyDead = null;
    }

    private void OnEnable()
    {
        _followTargetPosition.enabled = false;
    }

    public void DecrementLife(int amount)
    {
        _life -= amount;
        if(HasNoLife())
        {
            _score.IncrementScore(3);
            OnEnemyDead?.Invoke();
            ObjectPool.Instance.ReturnObjectToPool(gameObject, ObjectPool.PoolObjectType.Enemy);
        }
    }

    bool HasNoLife()
    {
        return _life <= 0;
    }

    public void SetLife(int value)
    {
        _life = value;
    }

    public void SubscribeToOnEnemyDead(Action action)
    {
        OnEnemyDead += action;
    }
}
