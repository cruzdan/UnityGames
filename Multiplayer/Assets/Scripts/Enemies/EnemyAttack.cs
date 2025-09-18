using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float timeBetweenAttacks;
    
    public virtual void Attack()
    {

    }
}
