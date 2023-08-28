using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] private int _life;
    [SerializeField] private int _lifeIncrement;
    public int GetLife()
    {
        return _life;
    }

    public void IncrementLife()
    {
        _life += _lifeIncrement;
    }

    public void SetLife(int life)
    {
        _life = life;
    }
}
