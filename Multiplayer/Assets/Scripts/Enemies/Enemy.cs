using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyState;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxLife;
    [SerializeField] private int currentLife;
    [SerializeField] private int maxDefense;
    [SerializeField] private int defense;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyDetection enemyDetection;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private Player playerTarget;
    public EnemyMovement EnemyMovement => enemyMovement;
    public EnemyAttack EnemyAttack => enemyAttack;
    public EnemyDetection EnemyDetection => enemyDetection;
    public EnemyState EnemyState => enemyState;
    public Player PlayerTarget { get { return playerTarget; } set { playerTarget = value; } }

    private void Update()
    {
        switch (enemyState.CurrentEnemyState)
        {
            case EnemyStateEnum.Idle:
                // Idle behavior
                IdleState();
                break;
            case EnemyStateEnum.Chasing:
                // Chasing behavior
                ChasingState();
                break;
            case EnemyStateEnum.Attacking:
                // Attacking behavior
                AttackingState();
                break;
        }
    }

    public void IdleState()
    {
        enemyDetection.Idle();
    }
    public void ChasingState()
    {
        enemyMovement.Chase();
    }
    public void AttackingState()
    {
        enemyAttack.Attack();
    }

    public void InitializeEnemy()
    {
        currentLife = maxLife;
        defense = maxDefense;
    }

    public void TakeDamage(int damage)
    {
        int damageAfterDefense = damage - defense;

        if (damageAfterDefense < 1)
            damageAfterDefense = 1;
        currentLife -= damageAfterDefense;
        if (currentLife <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        enemyObject.SetActive(false);
    }
}
