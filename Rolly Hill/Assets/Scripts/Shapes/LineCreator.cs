using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : Shape
{
    GameObject _block;
    Vector3 _blockPosition = Vector3.zero;
    public override void CreateShapeWithBlocks()
    {
        AssignBlockParentAndSetBlockPosition(0f, 0f);
        AssignBlockParentAndSetBlockPosition(-.4f, 0f);
        AssignBlockParentAndSetBlockPosition(-.8f, 0f);
        AssignBlockParentAndSetBlockPosition(.4f, 0f);
        AssignBlockParentAndSetBlockPosition(.8f, 0f);
    }

    void AssignBlockParentAndSetBlockPosition(float posX, float posY)
    {
        _blockPosition.x = posX;
        _blockPosition.y = posY;
        _block = BlockPool.Instance.GetObjectFromPool();
        _block.transform.parent = this.transform;
        _block.transform.localPosition = _blockPosition;
    }
}
