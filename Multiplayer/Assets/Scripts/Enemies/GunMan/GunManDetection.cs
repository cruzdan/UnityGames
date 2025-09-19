using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyState;

public class GunManDetection : EnemyDetection
{
    public override void Idle()
    {
        Debug.Log("Idle");
        Player nearestPlayer = GetNearestPlayer();
        if (nearestPlayer != null)
        {
            enemy.PlayerTarget = nearestPlayer;
            enemy.EnemyState.CurrentEnemyState = EnemyStateEnum.Chasing;
            enemy.EnemyMovement.StartChase();
        }
    }
}
