using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMobileElement : MonoBehaviour
{
    [SerializeField] private MoveRectTransformOnAxis _moveIn;
    [SerializeField] private MoveRectTransformOnAxis _moveOut;
    public void MoveIn()
    {
        _moveIn.enabled = true;
    }

    public void MoveOut()
    {
        _moveOut.enabled = true;
    }

    public void StopMoveIn()
    {
        _moveIn.enabled = false;
    }

    public void StopMoveOut()
    {
        _moveOut.enabled = false;
    }
}
