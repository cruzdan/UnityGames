using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//Se encarga de crear enemigos aleatorios asignados en posiciones aleatorias en un tiempo aleatorio
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<Transform> spawns;
    [SerializeField] private float spawnIntervalMin = 2f;
    [SerializeField] private float spawnIntervalMax = 5f;
    [SerializeField] private bool isSpawning = false;
    [SerializeField] private PlayerManager playerManager;
    private Coroutine spawningCoroutine;

    public List<GameObject> EnemyPrefabs => enemyPrefabs;
    public List<Transform> Spawns => spawns;
    public float SpawnIntervalMin { get => spawnIntervalMin; set => spawnIntervalMin = value; }
    public float SpawnIntervalMax { get => spawnIntervalMax; set => spawnIntervalMax = value; }
    [ContextMenu("StartSpawning")]
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
            SpawnRandomEnemyOnRandomSpawn();
        }
    }
    [ContextMenu("SpawnRandomEnemyOnRandomSpawn")]
    public void SpawnRandomEnemyOnRandomSpawn()
    {
        Debug.Log("SpawnRandomEnemyOnRandomSpawn");
        int spawnIndex = Random.Range(0, spawns.Count);
        int enemyIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyObject = Instantiate(enemyPrefabs[enemyIndex], spawns[spawnIndex].position, Quaternion.identity);
        enemyObject.GetComponent<Enemy>().PlayerManager = playerManager;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
        }
    }
}
