using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
//Class in charge of creating random enemies assigned in random positions at a random time
public class EnemyManager : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private float spawnIntervalMin = 2f;
    [SerializeField] private float spawnIntervalMax = 5f;
    [SerializeField] private bool isSpawning = false;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private int maxEnemiesToSpawn;
    #endregion
    #region Override
    [Header("Override Settings")]
    [SerializeField] public bool overrideEnemySpawn;
    [SerializeField] public int overrideEnemyIndexToSpawn;
    [SerializeField] private bool overrideSpawnAllEnemySequence;
    private int overrideSequenceSpawnIndex = 0;
    #endregion
    #region Private Variables
    private Coroutine spawningCoroutine;
    #endregion
    #region Static Variables
    public static int TotalCurrentEnemies;
    public static List<Enemy> enemiesSpawned = new List<Enemy>();
    #endregion
    #region Auxiliar Variables
    private Enemy enemy;
    #endregion
    #region Public Properties
    public float SpawnIntervalMin { get => spawnIntervalMin; set => spawnIntervalMin = value; }
    public float SpawnIntervalMax { get => spawnIntervalMax; set => spawnIntervalMax = value; }
    #endregion
    #region Functions
    public void StartSpawning()
    {
        isSpawning = true;
        spawningCoroutine = StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);
            if (TotalCurrentEnemies < maxEnemiesToSpawn)
            {
                if (overrideSpawnAllEnemySequence)
                {
                    SpawnEnemyAtPosition(overrideSequenceSpawnIndex, Spawns.Instance.GetSpawnByType(SpawnType.Enemy).position);

                    overrideSequenceSpawnIndex++;
                    if (overrideSequenceSpawnIndex >= Constants.ENEMY_NAMES.Count)
                    {
                        overrideSequenceSpawnIndex = 0;
                    }
                }
                else
                {
                    if (overrideEnemySpawn)
                    {
                        SpawnEnemyAtPosition(overrideEnemyIndexToSpawn, Spawns.Instance.GetSpawnByType(SpawnType.Enemy).position);
                    }
                    else
                    {
                        SpawnRandomEnemyAtRandomSpawn();
                    }
                }
            }
        }
    }

    public Enemy SpawnEnemyAtRandomSpawn(string enemyName)
    {
        return SpawnEnemyAtPosition(enemyName, Spawns.Instance.GetSpawnByType(SpawnType.Enemy).position);
    }

    public Enemy SpawnRandomEnemyAtRandomSpawn()
    {
        int enemyIndex = Random.Range(0, Constants.ENEMY_NAMES.Count);
        return SpawnEnemyAtPosition(enemyIndex, Spawns.Instance.GetSpawnByType(SpawnType.Enemy).position);
    }

    Enemy SpawnEnemyAtPosition(int enemyIndex, Vector3 position)
    {
        NetworkObject enemyObject = GameNetwork.Instance.Spawn(Constants.ENEMY_NAMES[enemyIndex],
            position, Quaternion.identity);
        return HandleEnemySpawn(enemyObject);
    }

    Enemy SpawnEnemyAtPosition(string enemyName, Vector3 position)
    {
        NetworkObject enemyObject = GameNetwork.Instance.Spawn(enemyName, position, Quaternion.identity);
        return HandleEnemySpawn(enemyObject);
    }

    Enemy HandleEnemySpawn(NetworkObject enemyObject)
    {
        enemy = enemyObject.GetComponent<Enemy>();
        enemy.PlayerManager = playerManager;
        enemy.Initialize();
        enemiesSpawned.Add(enemy);
        TotalCurrentEnemies++;
        return enemy;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        CoroutineExtensions.StopCoroutineSafe(this, ref spawningCoroutine);
    }

    public static void DecreaseEnemyCount()
    {
        TotalCurrentEnemies--;
        if (TotalCurrentEnemies < 0) TotalCurrentEnemies = 0;
    }

    public static void RemoveEnemySpawned(Enemy enemy)
    {
        enemiesSpawned.Remove(enemy);
    }

    public static void OnPlayerDisconnected(Player player)
    {
        foreach (Enemy enemy in enemiesSpawned)
        {
            if (enemy.PlayerTarget == player)
            {
                enemy.PlayerTarget = null;
                enemy.Initialize();
            }
        }
    }
    #endregion
}
