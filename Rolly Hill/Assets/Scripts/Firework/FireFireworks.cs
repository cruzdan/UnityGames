using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFireworks : MonoBehaviour
{
    [SerializeField] private FireworkLine[] _fireworkLines;

    public void ActivateFireworkLines()
    {
        int total = _fireworkLines.Length;
        for(int i = 0; i < total; i++)
        {
            _fireworkLines[i].Init();
        }
    }
}
