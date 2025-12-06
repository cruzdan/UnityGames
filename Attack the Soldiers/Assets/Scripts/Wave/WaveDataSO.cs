using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveDataSO")]
public class WaveDataSO : ScriptableObject
{
    #region General
    [Header("General Settings")]
    public int TotalWaves;
    public int TotalEnemiesInFirstWave;
    public int TimeBetweenWaves;
    public float TimeToSpawnEnemies;
    public List<string> InitialEnemies = new List<string>();
    public List<WaveEnemyAppearance> EnemiesToUnlock = new List<WaveEnemyAppearance>();
    [Header("Max Values")]
    public float MaxEnemySpeed;
    public float MaxEnemyDamage;
    public float MaxEnemyLife;
    public int MaxEnemiesPerWave;
    #endregion
    #region Scaling Rates Per Wave
    [Header("Enemy Scaling Rates Per Wave")]
    [Tooltip("Total enemies added to spawn in next waves")]
    public int EnemyIncreaseRatePerWave;
    public float EnemyLifeIncreaseRatePerWave;
    public float EnemyDamageIncreaseRatePerWave;
    public float EnemySpeedIncreaseRatePerWave;
    #endregion
}

[System.Serializable]
public class WaveEnemyAppearance
{
    public string EnemyName;
    public int AppearanceWave;
}
