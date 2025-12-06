using System.Collections.Generic;
using UnityEngine;
public class Spawns : Singleton<Spawns>
{
    #region Serialized Fields
    [SerializeField] private List<SpawnInfo> playerSpawnsInfo;
    #endregion
    #region Private Variables
    Dictionary<SpawnType, SpawnInfo> spawnInfoByType = new Dictionary<SpawnType, SpawnInfo>();
    #endregion
    #region Functions
    private void Awake()
    {
        spawnInfoByType.InitializeFromList(playerSpawnsInfo, info => info.spawnType);
    }

    public Transform GetSpawnByType(SpawnType spawnType)
    {
        if (spawnInfoByType.TryGetValue(spawnType, out SpawnInfo spawnInfo))
        {
            if (spawnInfo.IsTesting)
            {
                return spawnInfo.Transforms[spawnInfo.TestSpawnIndex];
            }
            return spawnInfo.Transforms[Random.Range(0, spawnInfo.Transforms.Length)];
        }
        Debug.LogError("Spawn type not found: " + spawnType);
        return null;
    }
    #endregion
}

[System.Serializable]
public class SpawnInfo
{
    public SpawnType spawnType;
    public Transform[] Transforms;
    public bool IsTesting;
    public int TestSpawnIndex;
}

public enum SpawnType
{
    Player,
    Box,
    Enemy
}