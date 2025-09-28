using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private BulletInfoSO bulletInfoSO;
    [SerializeField] private bool isOffline = true;

    public BulletInfoSO BulletInfoSO => bulletInfoSO;

    private void Awake()
    {
        bulletInfoSO.InitializeBulletInfoDictionary();
    }

    private void Start()
    {
        if (isOffline)
        {
            //playerTransform.position = Spawns.Instance.GetPlayerSpawnPoint().position;
            //enemyManager.StartSpawning();
        }
        else
        {

        }
        
    }
}
