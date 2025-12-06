using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private WaveDataSO waveDataSO;
    #endregion
    #region Private Variables
    private int currentWave = 0;
    private int enemiesAlive = 0;
    private List<string> activeEnemies = new List<string>();
    private Coroutine waveCoroutine;
    #endregion
    #region Public Properties
    public int CurrentWave { get { return currentWave; } set { currentWave = value; } }
    public int EnemiesAlive { get { return enemiesAlive; } set { enemiesAlive = value; } }
    public WaveDataSO WaveDataSO => waveDataSO;
    #endregion
    #region Actions
    public Action OnStartWaves;
    public Action<int> OnWaveCompleted;
    public Action OnAllWavesCompleted;
    #endregion
    #region Functions
    public void StartWaves()
    {
        Initialize();
        waveCoroutine = StartCoroutine(WaveFlow());
        OnStartWaves?.Invoke();
    }

    void Initialize()
    {
        activeEnemies.Clear();
        foreach (string enemyName in waveDataSO.InitialEnemies)
            activeEnemies.Add(enemyName);
    }

    private IEnumerator WaveFlow()
    {
        while (currentWave < waveDataSO.TotalWaves)
        {
            // 1. BREAK STAGE
            float breakTime = waveDataSO.TimeBetweenWaves;
            yield return new WaitForSeconds(breakTime);

            // 2. SPAWN STAGE
            int count = Mathf.Min(
                    waveDataSO.TotalEnemiesInFirstWave + waveDataSO.EnemyIncreaseRatePerWave * currentWave,
                    waveDataSO.MaxEnemiesPerWave);
            enemiesAlive = count;

            UnlockNewActiveEnemyIfPossible();
            for (int i = 0; i < count; i++)
            {
                Enemy enemy = enemyManager.SpawnEnemyAtRandomSpawn(GetRandomActiveEnemyName());
                enemy.Health.OnDie -= OnEnemyKilled;
                enemy.Health.OnDie += OnEnemyKilled;
                enemy.Health.CurrentLife = Mathf.Min(
                    enemy.Health.CurrentLife + waveDataSO.EnemyLifeIncreaseRatePerWave * currentWave,
                    waveDataSO.MaxEnemyLife);
                enemy.EnemyMovement.Speed = Mathf.Min(
                    enemy.EnemyMovement.Speed + waveDataSO.EnemySpeedIncreaseRatePerWave * currentWave,
                    waveDataSO.MaxEnemySpeed);
                enemy.EnemyAttack.Damage = (int)Mathf.Min(
                    enemy.EnemyAttack.Damage + waveDataSO.EnemyDamageIncreaseRatePerWave * currentWave,
                    waveDataSO.MaxEnemyDamage);
                enemy.EnemyAttack.SetDamage(enemy.EnemyAttack.Damage);
                yield return new WaitForSeconds(waveDataSO.TimeToSpawnEnemies);
            }
            // 3. COMBAT STAGE - WAIT UNTIL ALL DEAD
            while (enemiesAlive > 0)
                yield return null;
            currentWave++;
            if (currentWave < waveDataSO.TotalWaves)
                OnWaveCompleted?.Invoke(currentWave);
        }
        OnAllWavesCompleted?.Invoke();
    }

    void UnlockNewActiveEnemyIfPossible()
    {
        foreach (WaveEnemyAppearance waveEnemyAppearance in waveDataSO.EnemiesToUnlock)
        {
            if (waveEnemyAppearance.AppearanceWave == currentWave + 1 &&
                !activeEnemies.Contains(waveEnemyAppearance.EnemyName))
            {
                activeEnemies.Add(waveEnemyAppearance.EnemyName);
            }
        }
    }

    string GetRandomActiveEnemyName()
    {
        return activeEnemies[UnityEngine.Random.Range(0, activeEnemies.Count)];
    }

    void OnEnemyKilled()
    {
        enemiesAlive--;
    }
    #endregion
}
