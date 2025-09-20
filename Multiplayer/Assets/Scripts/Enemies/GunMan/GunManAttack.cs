using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManAttack : EnemyAttack
{
    public override void Attack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            enemy.PlayerTarget.DecrementLife(damage);
            timer = attackCooldown;
        }
    }
    public override void StartAttack()
    {
        timer = 0;
    }

}
