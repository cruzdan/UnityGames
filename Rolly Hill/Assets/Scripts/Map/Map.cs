using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static event Action<int> OnLevelChanged;
    public static event Action<int> OnTotalBlocksChanged;
    [SerializeField] private GameObject[] _mapPrefabs;
    [SerializeField] private int _level = 1;
    [SerializeField] private BlockCounter ballBlockCounter;
    private GameObject _currentMap;

    void Start()
    {
        _level = PlayerPrefs.GetInt("Level", 1);
        _level--;
        OnPassLevel();
        LoosingPhase.OnLoose += OnLooseGame;
        GameManager.OnPassLevel += OnPassLevel;
    }

    void CreateMap(int mapIndex)
    {
        _currentMap = Instantiate(_mapPrefabs[mapIndex]);
        MapBlocksCreator mapCreator = _currentMap.GetComponent<MapBlocksCreator>();
        mapCreator.CreateMapBlocks();
        mapCreator.AddCounterToPathChecks(ballBlockCounter);
    }

    private void OnDisable()
    {
        LoosingPhase.OnLoose -= OnLooseGame;
        GameManager.OnPassLevel -= OnPassLevel;
    }
    void OnLooseGame()
    {
        _currentMap.GetComponent<MapBlocksCreator>().CreateMapBlocks();
        switch (_level)
        {
            case 3:
            case 5:
                _currentMap.GetComponent<MapResetter>().ResetMap();
                break;
        }
    }

    void OnPassLevel()
    {
        Destroy(_currentMap);
        int mapIndex;
        _level++;
        mapIndex = (_level - 1) % 6;
        CreateMap(mapIndex);
        switch (mapIndex)
        {
            case 2:
            case 4:
                _currentMap.GetComponent<MapResetter>().ResetMap();
                break;
        }
        OnLevelChanged?.Invoke(_level);
        int totalBLocks = _currentMap.GetComponent<MapBlocksCreator>().GetTotalBlocks();
        OnTotalBlocksChanged?.Invoke(totalBLocks);
        SaveLevel();
    }

    void SaveLevel()
    {
        PlayerPrefs.SetInt("Level", _level);
    }

    public int GetTotalBlocks()
    {
        return _currentMap.GetComponent<MapBlocksCreator>().GetTotalBlocks();
    }
}
