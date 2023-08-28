using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderManager : MonoBehaviour
{
    [SerializeField] private int _firstCylinderLifes;
    [SerializeField] private int _lifesToIncrementByLevel;
    [SerializeField] private int _dispersionLifes;

    public int GetFirstCylinderLife()
    {
        return _firstCylinderLifes;
    }

    public int GetLifesInLevel(int level)
    {
        return _firstCylinderLifes + _lifesToIncrementByLevel * level;
    }

    public int GetLifesInLevelWithDispersion(int level)
    {
        return _firstCylinderLifes + _lifesToIncrementByLevel * (level - 1) - Random.Range(0, _dispersionLifes + 1);
    }
}
