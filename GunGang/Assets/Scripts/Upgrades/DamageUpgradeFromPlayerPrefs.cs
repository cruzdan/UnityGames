using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpgradeFromPlayerPrefs : MonoBehaviour
{
    [SerializeField] private int _damageIncrement;
    [SerializeField] private Bullet _bullet;

    private void Awake()
    {
        InitDamageFromPlayerPrefs();
    }

    void InitDamageFromPlayerPrefs()
    {
        SetBulletDamage(PlayerPrefs.GetInt("Damage", 1));
    }

    void SetBulletDamage(int damage)
    {
        _bullet.SetDamage(damage);
    }

    public void IncrementDamage()
    {
        _bullet.SetDamage(_bullet.GetDamage() + _damageIncrement);
        SaveDamage();
    }

    public void SaveDamage()
    {
        PlayerPrefs.SetInt("Damage", _bullet.GetDamage());
        PlayerPrefs.Save();
    }
}
