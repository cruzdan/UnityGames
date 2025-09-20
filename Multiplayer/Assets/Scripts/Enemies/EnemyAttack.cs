using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float timer;
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public virtual void Attack()
    {
    }

    public virtual void StartAttack()
    {
    }
}
