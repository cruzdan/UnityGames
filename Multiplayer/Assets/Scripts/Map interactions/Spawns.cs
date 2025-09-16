using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawns : Singleton<Spawns>
{
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private Transform[] boxSpawnPoints;
    private int playerSpawnIndex = -1;
    public Transform GetPlayerSpawnPoint()
    {
        if (playerSpawnIndex >= playerSpawnPoints.Length - 1)
        {
            playerSpawnIndex = 0;
        }
        else
        {
            playerSpawnIndex++;
        }
        return playerSpawnPoints[playerSpawnIndex];
    }
    public Transform GetBoxSpawnPoint()
    {
        return boxSpawnPoints[Random.Range(0,boxSpawnPoints.Length)];
    }
}