using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    [SerializeField] private GameEvent OnFireworksEnd;

    [SerializeField] private GameObject[] _fireworks;
    [SerializeField] float _fireworkDurationTime = 5;
    float _timer = 0;

    private void Start()
    {
        PlayerSpeedDecrementer.OnFireworksAppear += AppearFireworks;
        enabled = false;
    }

    private void OnDestroy()
    {
        PlayerSpeedDecrementer.OnFireworksAppear -= AppearFireworks;
    }
    public void AppearFireworks(float positionZ)
    {
        enabled = true;
        SetFireworkPosition(0, new Vector3(-2.5f, 1, positionZ));
        SetFireworkPosition(1, new Vector3(2.5f, 1, positionZ));
        EnableFireworks();   
    }

    void SetFireworkPosition(int fireworkIndex, Vector3 position)
    {
        _fireworks[fireworkIndex].transform.position = position;
    }

    void EnableFireworks()
    {
        _fireworks[0].SetActive(true);
        _fireworks[1].SetActive(true);
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _fireworkDurationTime)
        {
            QuitFireworks();
            OnFireworksEnd.TriggerEvent();
        }
    }

    public void QuitFireworks()
    {
        ResetTimer();
        DisableFireworks();
        enabled = false;
    }

    void ResetTimer()
    {
        _timer = 0;
    }

    void DisableFireworks()
    {
        _fireworks[0].SetActive(false);
        _fireworks[1].SetActive(false);
    }
}
