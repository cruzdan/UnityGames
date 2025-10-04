using UnityEngine;
public class Spawns : Singleton<Spawns>
{
    #region Serialized Fields
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private Transform[] boxSpawnPoints;
    #endregion
    #region Private Variables
    private int playerSpawnIndex = -1;
    #endregion
    #region Functions
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
    #endregion
}