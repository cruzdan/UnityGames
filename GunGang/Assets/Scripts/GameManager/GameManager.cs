using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameEvent OnPassLevel;
    [SerializeField] private GameEvent OnRestartLevel;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _playerCharacterParent;

    void Start()
    {
        Application.targetFrameRate = 30;
    }

    public GameObject GetPlayer()
    {
        return _player;
    }

    public Transform GetPlayerCharacterParent()
    {
        return _playerCharacterParent;
    }

    public void PassLevel()
    {
        OnPassLevel.TriggerEvent();
    }

    public void RestartLevel()
    {
        OnRestartLevel.TriggerEvent();
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void ContinueTime()
    {
        Time.timeScale = 1;
    }
}
