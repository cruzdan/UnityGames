using System.Collections;
using Unity.Netcode;
using UnityEngine;
public class KnockoutAttack : EnemyAttack
{
    #region Serialized Variables
    [SerializeField] private float attackAnimationDuration = 0.2f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private GameObject attackObject;
    [SerializeField] private EnemyTouchAttack enemyTouchAttack;
    [SerializeField, Tooltip("Time it will take for the enemy to stun the player")] private float stunTime;
    [SerializeField, Tooltip("Time the player will remain stunned")] private float stunTimeToPlayer;
    #endregion
    #region Private Variables
    private float stunTimer;
    private bool canStun = true;
    private Coroutine attackCoroutine;
    #endregion
    #region Functions
    public override void SetDamage(int damage)
    {
        base.SetDamage(damage);
    }

    public override void StartAttack()
    {
        attackCoroutine = StartCoroutine(Knockout());
    }

    public override void Attack()
    {
        enemy.EnemyMovement.FlipEnemyDirectionIfPossible();
        UpdateStunTimer();
    }

    IEnumerator Knockout()
    {
        while (!TargetIsOutOfRange())
        {
            //enemy.Animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationDuration);
            if (GameNetwork.Instance.IsOnline)
                ActiveAttackObjectClientRpc(true);
            else
                ActiveAttackObject(true);
            yield return new WaitForSeconds(attackDelay/2f);
            AttackTargetIfPossible();
            yield return new WaitForSeconds(attackDelay/2f);
            if (GameNetwork.Instance.IsOnline)
                ActiveAttackObjectClientRpc(false);
            else
                ActiveAttackObject(false);
            //enemy.Animator.SetTrigger("AttackCooldown");
            yield return new WaitForSeconds(attackCooldown);
        }
        enemy.StartChase();
        CoroutineExtensions.StopCoroutineSafe(this, ref attackCoroutine);
    }

    [ClientRpc(RequireOwnership = false)]
    void ActiveAttackObjectClientRpc(bool value)
    {
        ActiveAttackObject(value);
    }

    void ActiveAttackObject(bool value)
    {
        attackObject.SetActive(value);
    }

    void AttackTargetIfPossible()
    {
        if (TargetIsAbleToAttack())
        {
            StunPlayerIfPossible();
            if (GameNetwork.Instance.IsOnline)
                enemy.PlayerTarget.DecrementLifeClientRpc(damage);
            else
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
            if (GameNetwork.Instance.IsOnline)
                enemy.PlayerTarget.PlayerStun.StartStunPlayerCoroutineClientRpc(stunTimeToPlayer);
            else
                enemy.PlayerTarget.PlayerStun.StartStunPlayerCoroutine(stunTimeToPlayer);
            canStun = false;
            stunTimer = stunTime;
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
    #endregion
}
