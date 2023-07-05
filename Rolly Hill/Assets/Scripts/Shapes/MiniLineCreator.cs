using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MiniLineCreator : Shape
{
    GameObject _block;
    Vector3 _blockPosition = Vector3.zero;

    public override void CreateShapeWithBlocks()
    {
        AssignBlockParentAndSetBlockPosition(0);
        AssignBlockParentAndSetBlockPosition(-.4f);
        AssignBlockParentAndSetBlockPosition(.4f);
    }

    void AssignBlockParentAndSetBlockPosition(float posX)
    {
        _blockPosition.x = posX;
        _block = BlockPool.Instance.GetObjectFromPool();
        _block.transform.parent = this.transform;
        _block.transform.localPosition = _blockPosition;
    }
}
