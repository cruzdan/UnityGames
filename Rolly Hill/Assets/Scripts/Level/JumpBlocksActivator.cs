using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBlocksActivator : MonoBehaviour
{
    [SerializeField] private JumpingBlock[] _jumpingBlocks;
    [SerializeField] private float _timeToJump;
    private float _timer;
    int _totalBlocks;
    int _currentBlock;
    public void Init()
    {
        _totalBlocks = _jumpingBlocks.Length;
        _currentBlock = 0;
    }
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _timeToJump)
        {
            _timer = 0;
            ActivateCurrentJumpingBlock();
            _currentBlock++;
            if(HasActivatedAllBlocks())
            {
                this.enabled = false;
            }
        }
    }

    void ActivateCurrentJumpingBlock()
    {
        _jumpingBlocks[_currentBlock].enabled = true;
        _jumpingBlocks[_currentBlock].Jump();
    }

    bool HasActivatedAllBlocks()
    {
        return _currentBlock >= _totalBlocks;
    }
}
