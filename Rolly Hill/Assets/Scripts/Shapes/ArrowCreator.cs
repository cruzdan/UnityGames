using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArrowCreator : Shape
{
    GameObject _block;
    Vector3 _blockPosition;
    public override void CreateShapeWithBlocks()
    {
        _blockPosition.y = 0;

        AssignBlockParentAndSetBlockPosition(0f, 1.2f);
        AssignBlockParentAndSetBlockPosition(-0.2f, 0.8f);
        AssignBlockParentAndSetBlockPosition(0.2f, 0.8f);
        AssignBlockParentAndSetBlockPosition(0f, 0.4f);
        AssignBlockParentAndSetBlockPosition(-0.4f, 0.4f);
        AssignBlockParentAndSetBlockPosition(0.4f, 0.4f);
        AssignBlockParentAndSetBlockPosition(0f, 0f);
        AssignBlockParentAndSetBlockPosition(-0.4f, 0f);
        AssignBlockParentAndSetBlockPosition(0.4f, 0f);
        AssignBlockParentAndSetBlockPosition(0.8f, 0f);
        AssignBlockParentAndSetBlockPosition(-0.8f, 0f);
    }

    void AssignBlockParentAndSetBlockPosition(float posX, float posZ)
    {
        _blockPosition.x = posX;
        _blockPosition.z = posZ;
        _block = BlockPool.Instance.GetObjectFromPool();
        _block.transform.parent = this.transform;
        _block.transform.localPosition = _blockPosition;
    }
}
