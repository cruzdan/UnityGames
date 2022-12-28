using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyBehaviour : GhostBehaviour
{
    Vector2Int nextMSDPosition = new();
    Vector2Int msPacManTargetPos;
    Vector2Int blinkyPos;
    [SerializeField] private Transform blinky;
    public override void InitOnStartGameVariables()
    {
        SetSpeed(LevelInformation.Instance.GhostSpeed);
        SetPause(false);
    }
    public override Vector2Int GetTargetPosition()
    {
        blinkyPos.x = (int)blinky.position.x;
        blinkyPos.y = -(int)blinky.position.y;
        msPacManTargetPos = gameManager.GetMsPacManPositionOnGrid() + GetMsPCNextPosition(gameManager.GetMsPacManDirection());
        return blinkyPos + ((msPacManTargetPos - blinkyPos) * 2);
    }
    Vector2Int GetMsPCNextPosition(int msDirection)
    {
        switch (msDirection)
        {
            case 0:
                nextMSDPosition.x = -2;
                nextMSDPosition.y = -2;
                return nextMSDPosition;
            case 1:
                nextMSDPosition.x = 2;
                nextMSDPosition.y = 0;
                return nextMSDPosition;
            case 2:
                nextMSDPosition.x = 0;
                nextMSDPosition.y = 2;
                return nextMSDPosition;
            case 3:
                nextMSDPosition.x = -2;
                nextMSDPosition.y = 0;
                return nextMSDPosition;
            default: return Vector2Int.zero;
        }
    }
}
