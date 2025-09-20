using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private bool isOffline = true;

    private void Start()
    {
        if (isOffline)
        {
            //enemyManager.StartSpawning();
        }
    }
}
