using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkyBehaviour : GhostBehaviour
{
    Vector2Int nextMSDPosition = new();
    public override void InitOnStartGameVariables()
    {
        SetSpeed(LevelInformation.Instance.GhostSpeed);
        SetPause(false);
    }
    public override Vector2Int GetTargetPosition()
    {
        return gameManager.GetMsPacManPositionOnGrid() + GetMsPCNextPosition(gameManager.GetMsPacManDirection());
    }
    Vector2Int GetMsPCNextPosition(int msDirection)
    {
        switch (msDirection)
        {
            case 0:
                nextMSDPosition.x = -4;
                nextMSDPosition.y = -4;
                return nextMSDPosition;
            case 1:
                nextMSDPosition.x = 4;
                nextMSDPosition.y = 0;
                return nextMSDPosition;
            case 2:
                nextMSDPosition.x = 0;
                nextMSDPosition.y = 4;
                return nextMSDPosition;
            case 3:
                nextMSDPosition.x = -4;
                nextMSDPosition.y = 0;
                return nextMSDPosition;
            default: return Vector2Int.zero;
        }
    }
}
