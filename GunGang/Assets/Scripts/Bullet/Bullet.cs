using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet")]
public class Bullet : ScriptableObject
{
    [SerializeField] private int _damage;
    [SerializeField] private float _timeToShoot;

    public int GetDamage()
    {
        return _damage;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public float GetTimeToShoot()
    {
        return _timeToShoot;
    }

    public void SetTimeToShoot(float time)
    {
        _timeToShoot = time;
    }
}
