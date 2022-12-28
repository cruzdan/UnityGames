using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkyBehaviour : GhostBehaviour
{
    bool elroyIsActive = false;
    int elroyPhase;
    public override void InitOnStartGameVariables()
    {
        SetActiveGridMovement(true);
        InitGridMovement(MovementForGrid.MovementType.RandomMovement, false, false);
    }
    public override Vector2Int GetTargetPosition()
    {
        return gameManager.GetMsPacManPositionOnGrid();
    }
    public override void ChangeMovementToScatter()
    {
        if (elroyIsActive)
        {
            ChangeMovementToChase();
        }
        else
        {
            base.ChangeMovementToScatter();
        }
    }
    public void SetElroyActive(bool value)
    {
        elroyIsActive = value;
    }
    public bool GetElroyActive()
    {
        return elroyIsActive;
    }
    public void ChangeModeToElroy()
    {
        switch (movementType)
        {
            case MovementType.Scatter:
            case MovementType.RandomMovement:
                ChangeMovementToChase();
                break;
        }
    }
    public override void ReturnFromFrightToScatter()
    {
        base.ReturnFromFrightToScatter();
        SetElroySpeed();
    }
    public override void ReturnFromFrightToChase()
    {
        base.ReturnFromFrightToChase();
        SetElroySpeed();
    }
    public override void ChangeGhostSpeed(float newSpeed)
    {
        if (elroyIsActive)
        {
            if (elroyPhase == 1)
            {
                SetSpeed(LevelInformation.Instance.Elroy1Speed);
                SetGridSpeed(LevelInformation.Instance.Elroy1Speed);
            }
            else
            {
                SetSpeed(LevelInformation.Instance.Elroy2Speed);
                SetGridSpeed(LevelInformation.Instance.Elroy2Speed);
            }
        }
        else
        {
            base.ChangeGhostSpeed(newSpeed);
        }
    }
    void SetElroySpeed()
    {
        if (elroyIsActive)
        {
            if (elroyPhase == 1)
            {
                SetSpeed(LevelInformation.Instance.Elroy1Speed);
                SetGridSpeed(LevelInformation.Instance.Elroy1Speed);
            }
            else
            {
                SetSpeed(LevelInformation.Instance.Elroy2Speed);
                SetGridSpeed(LevelInformation.Instance.Elroy2Speed);
            }
        }
    }
    public void SetElroyPhase(int newPhase)
    {
        elroyPhase = newPhase;
    }
}
