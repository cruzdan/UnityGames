using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Class in charge of creating random enemies assigned in random positions at a random time
public class EnemyManager : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<Transform> spawns;
    [SerializeField] private float spawnIntervalMin = 2f;
    [SerializeField] private float spawnIntervalMax = 5f;
    [SerializeField] private bool isSpawning = false;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private int maxEnemiesToSpawn;
    #endregion
    #region Private Variables
    private Coroutine spawningCoroutine;
    #endregion
    #region Static Variables
    public static int TotalCurrentEnemies;
    #endregion
    #region Auxiliar Variables
    private Enemy enemy;
    #endregion
    #region Public Properties
    public List<GameObject> EnemyPrefabs => enemyPrefabs;
    public List<Transform> Spawns => spawns;
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
                SpawnRandomEnemyOnRandomSpawn();
        }
    }
    public void SpawnRandomEnemyOnRandomSpawn()
    {
        int spawnIndex = Random.Range(0, spawns.Count);
        int enemyIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyObject = ObjectPool.Singleton.GetObject(enemyPrefabs[enemyIndex].name,
            spawns[spawnIndex].position, Quaternion.identity);
        enemy = enemyObject.GetComponent<Enemy>();
        enemy.PlayerManager = playerManager;
        enemy.Health.InitializeHealth();
        TotalCurrentEnemies++;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
        }
    }

    public static void DecreaseEnemyCount()
    {
        TotalCurrentEnemies--;
        if (TotalCurrentEnemies < 0) TotalCurrentEnemies = 0;
    }
    #endregion
}
