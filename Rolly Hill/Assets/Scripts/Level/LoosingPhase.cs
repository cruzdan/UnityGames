using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoosingPhase : MonoBehaviour
{
    public static event Action OnLoose;
    [SerializeField] private float _looseTime;
    private float _timer;

    private void Start()
    {
        DeathTrigger.OnDeadPhase += Activate;
        enabled = false;
    }
    private void OnDestroy()
    {
        DeathTrigger.OnDeadPhase -= Activate;
    }
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _looseTime)
        {
            OnLoose?.Invoke();
            _timer = 0;
            enabled = false;
        }
    }

    void Activate()
    {
        enabled = true;
    }
}
