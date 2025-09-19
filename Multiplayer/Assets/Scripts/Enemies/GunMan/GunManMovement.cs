using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManMovement : EnemyMovement
{
    //test
    [SerializeField] private float cooldown;
    [SerializeField] private float timer;
    public override void StartChase()
    {
        timer = cooldown;
    }
    public override void Chase()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            enemy.EnemyState.CurrentEnemyState = EnemyState.EnemyStateEnum.Attacking;
            enemy.EnemyAttack.StartAttack();
        }
    }
}
