using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCylinders : MonoBehaviour
{
    [SerializeField] private Transform[] _cylinders;
    [SerializeField] private MoveOnAxisUntilReachTarget[] _cylinderMovements;
    public void ResetCylinderZPositions()
    {
        Vector3 newPosition;
        int total = _cylinders.Length;
        for(int i = 0; i < total; i++)
        {
            newPosition = _cylinders[i].localPosition;
            newPosition.z = 0;
            _cylinders[i].localPosition = newPosition;
        }
    }

    public void InitCylinderMovements()
    {
        int total = _cylinderMovements.Length;
        for (int i = 0; i < total; i++)
        {
            _cylinderMovements[i].Init();
        }
    }
}
