using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map3Resetter : MapResetter
{
    [SerializeField] private JumpingBlock[] _jumpingBlockObjects;
    override public void ResetMap()
    {
        int total = _jumpingBlockObjects.Length;
        for(int i = 0; i < total; i++)
        {
            _jumpingBlockObjects[i].gameObject.SetActive(true);
            _jumpingBlockObjects[i].transform.position = _positions[i];
            _jumpingBlockObjects[i].ResetVariables();
        }
    }
}
