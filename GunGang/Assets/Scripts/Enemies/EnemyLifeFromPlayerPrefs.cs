using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeFromPlayerPrefs : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void Start()
    {
        InitEnemyLifeFromPlayerPrefs();
    }

    void InitEnemyLifeFromPlayerPrefs()
    {
        _enemy.SetLife(PlayerPrefs.GetInt("EnemyLife", 2));
    }

    public void SaveEnemyLife()
    {
        PlayerPrefs.SetInt("EnemyLife", _enemy.GetLife());
        PlayerPrefs.Save();
    }
}
