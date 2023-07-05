using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCubeCreator : Shape
{
    GameObject _block;
    Vector3 _blockPosition = Vector3.zero;

    public override void CreateShapeWithBlocks()
    {
        AssignBlockParentAndSetBlockPosition(-.2f, -.2f);
        AssignBlockParentAndSetBlockPosition(-.2f, .2f);
        AssignBlockParentAndSetBlockPosition(.2f, .2f);
        AssignBlockParentAndSetBlockPosition(.2f, -.2f);
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
