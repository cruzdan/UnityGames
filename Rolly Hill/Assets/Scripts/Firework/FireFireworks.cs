using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFireworks : MonoBehaviour
{
    [SerializeField] private FireworkLine[] _fireworkLines;
    void Start()
    {
        PlayerSpeedDecrementer.OnPlayerStops += ActivateFireworkLines;
    }

    private void OnDestroy()
    {
        PlayerSpeedDecrementer.OnPlayerStops -= ActivateFireworkLines;
    }

    void ActivateFireworkLines()
    {
        int total = _fireworkLines.Length;
        for(int i = 0; i < total; i++)
        {
            _fireworkLines[i].Init();
        }
    }
}
