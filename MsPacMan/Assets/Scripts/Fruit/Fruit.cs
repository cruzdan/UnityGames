using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    float timer;
    [SerializeField] private float timeToStartTargetMovement;
    [SerializeField] private MovementForGrid movementForGrid;
    [SerializeField] private FruitSpriteMovement anim;
    [SerializeField] private int gameIndex;
    private Vector2Int tunnelEntrance;
    bool tunnelReached;
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            movementForGrid.SetMovementType(MovementForGrid.MovementType.TargetMovement);
            movementForGrid.SetHasForbiddenTiles(false);
            enabled = false;
        }
        if (!tunnelReached)
        {
            if((int)transform.position.x == tunnelEntrance.x && -(int)transform.position.y == tunnelEntrance.y)
            {
                tunnelReached = true;
                movementForGrid.SetHasForbiddenTiles(true);
            }
        }
    }

    public void Init()
    {
        timer = timeToStartTargetMovement;
        movementForGrid.SetMovementType(MovementForGrid.MovementType.RandomMovement);
        movementForGrid.SetHasForbiddenTiles(false);
        movementForGrid.SetOnTunnel(true);
        movementForGrid.SetForbiddenTiles(LevelInformation.Instance.TunnelEntrances);
        movementForGrid.RestartLastPositionChanged();
        movementForGrid.enabled = true;
        enabled = true;
        SetActiveAnimation(true);
        tunnelReached = false;
    }
    public int GetScore()
    {
        return LevelInformation.Instance.FruitTypes[gameIndex] switch
        {
            0 => 100,
            1 => 200,
            2 => 500,
            3 => 700,
            4 => 1000,
            5 => 2000,
            6 => 5000,
            _ => 0,
        };
    }
    public int GetGameIndex()
    {
        return gameIndex;
    }
    public void SetActiveGridMovement(bool value)
    {
        movementForGrid.enabled = value;
    }
    public void SetActiveAnimation(bool value)
    {
        anim.enabled = value;
    }
    public void SetTunnelEntrance(Vector2Int newPosition)
    {
        tunnelEntrance = newPosition;
    }
}
