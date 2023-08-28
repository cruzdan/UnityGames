using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent _gameEvent;
    [SerializeField] private UnityEvent _onEventTriggered;

    private void OnEnable()
    {
        _gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.RemoveListener(this);
    }

    public void OnEventTriggered()
    {
        _onEventTriggered.Invoke();
    }
}
