using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPhaseData : MonoBehaviour
{
    [SerializeField] private int phase;
    public void SetPhase(int newPhase)
    {
        phase = newPhase;
    }

    public int GetPhase()
    {
        return phase;
    }
}
