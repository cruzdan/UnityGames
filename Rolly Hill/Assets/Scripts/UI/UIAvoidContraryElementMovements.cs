using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAvoidContraryElementMovements : MonoBehaviour
{
    [SerializeField] UIMobileElement[] _movingElements;

    public void StopElementMoves()
    {
        int total = _movingElements.Length;
        for (int i = 0; i < total; i++)
        {
            _movingElements[i].StopMoveIn();
            _movingElements[i].StopMoveOut();
        }
        
    }
}
