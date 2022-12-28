using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SueBehaviour : GhostBehaviour
{
    Vector2Int position = new ();
    public override void InitOnStartGameVariables()
    {
        SetSpeed(LevelInformation.Instance.GhostSpeed);
        SetPause(false);
    }
    public override Vector2Int GetTargetPosition()
    {
        position.x = (int)transform.position.x;
        position.y = -(int)transform.position.y;
        if((position - gameManager.GetMsPacManPositionOnGrid()).magnitude <= 8)
        {
            return targetScatterPosition;
        }
        else
        {
            return gameManager.GetMsPacManPositionOnGrid();
        }
    }
}
