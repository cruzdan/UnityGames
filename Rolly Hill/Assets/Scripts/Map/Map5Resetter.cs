using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map5Resetter : MapResetter
{
    [SerializeField] private HitExplosion[] _wallBlocks;
    override public void ResetMap()
    {
        int total = _wallBlocks.Length;
        for(int i = 0; i < total; i++)
        {
            _wallBlocks[i].ResetRigidbodyVelocity();
            _wallBlocks[i].transform.SetPositionAndRotation(_positions[i], Quaternion.identity);
        }
    }
}
