using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class KnockoutAttack : EnemyAttack
{
    [SerializeField] private float attackAnimationDuration = 0.2f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private GameObject attackObject;
    [SerializeField, Tooltip("Time it will take for the enemy to stun the player")] private float stunTime;
    [SerializeField, Tooltip("Time the player will remain stunned")] private float stunTimeToPlayer;
    private float stunTimer;
    bool canStun = true;

    private Coroutine attackCoroutine;
    public override void StartAttack()
    {
        attackCoroutine = StartCoroutine(Knockout());
    }
    public override void Attack()
    {
        enemy.EnemyMovement.FlipEnemyDirectionIfPossible();
        UpdateStunTimer();
        //if (TargetIsOutOfRange())
        //{
        //    enemy.StartChase();
        //    return;
        //}
        //timer -= Time.deltaTime;
        //if (timer <= 0)
        //{
        //    if (enemy.PlayerTarget != null)
        //    {
        //        enemy.PlayerTarget.DecrementLife(damage);
        //        timer = attackCooldown;
        //    }
        //}
    }
    IEnumerator Knockout()
    {
        while (!TargetIsOutOfRange())
        {
            //enemy.Animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationDuration);
            attackObject.SetActive(true);
            yield return new WaitForSeconds(attackDelay/2f);
            AttackTargetIfPossible();
            yield return new WaitForSeconds(attackDelay/2f);
            attackObject.SetActive(false);
            //enemy.Animator.SetTrigger("AttackCooldown");
            yield return new WaitForSeconds(attackCooldown);
        }
        enemy.StartChase();
        DisposeAttackCoroutine();
    }

    void AttackTargetIfPossible()
    {
        if (TargetIsAbleToAttack())
        {
            StunPlayerIfPossible();
            enemy.PlayerTarget.DecrementLife(damage);
        }
    }

    bool TargetIsAbleToAttack()
    {
        return TargetIsNearby() && TargetIsNearbyInX();
    }

    void StunPlayerIfPossible()
    {
        if (canStun)
        {
            enemy.PlayerTarget.PlayerStun.StartStunPlayer(stunTimeToPlayer);
            canStun = false;
            stunTimer = stunTime;
        }
    }

    void DisposeAttackCoroutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    void UpdateStunTimer()
    {
        if (!canStun)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                canStun = true;
            }
        }
    }
}
