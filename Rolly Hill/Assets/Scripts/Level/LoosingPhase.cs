using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoosingPhase : MonoBehaviour
{
    [SerializeField] private GameEvent OnLoose;
    [SerializeField] private float _looseTime;
    private float _timer;

    private void Start()
    {
        enabled = false;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _looseTime)
        {
            OnLoose.TriggerEvent();
            _timer = 0;
            enabled = false;
        }
    }
}
