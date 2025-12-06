using System.Collections;
using UnityEngine;
public class FasterAttack : EnemyAttack
{
    #region Serialized Variables
    [SerializeField] private float prepareAttackAnimationDuration = 0.2f;
    [SerializeField, Tooltip("Time to wait after attack and before start next attack")] 
    private float attackDelay = 0.1f;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackObject;
    [SerializeField] private EnemyTouchAttack enemyTouchAttack;
    #endregion
    #region Private Variables
    private float distanceTraveled;
    private Coroutine attackCoroutine;
    #endregion
    #region Functions
    public override void SetDamage(int damage)
    {
        base.SetDamage(damage);
        enemyTouchAttack.Damage = damage;
    }

    private void Start()
    {
        enemyTouchAttack.Damage = damage;
    }

    public override void StartAttack()
    {
        enemy.EnemyMovement.FlipEnemyDirectionIfPossible();
        attackCoroutine = StartCoroutine(FastAttack());
    }

    public override void Attack()
    {
    }

    IEnumerator FastAttack()
    {
        while (!TargetIsOutOfRange())
        {
            //enemy.Animator.SetTrigger("Attack");
            yield return new WaitForSeconds(prepareAttackAnimationDuration);
            //attackObject.SetActive(true);
            yield return AttackTargetIfPossible();
            yield return new WaitForSeconds(attackDelay);
            //attackObject.SetActive(false);
            //enemy.Animator.SetTrigger("AttackCooldown");
            yield return new WaitForSeconds(attackCooldown);
            enemy.EnemyMovement.FlipEnemyDirectionIfPossible();
        }
        enemy.StartChase();
        CoroutineExtensions.StopCoroutineSafe(this, ref attackCoroutine);
    }

    IEnumerator AttackTargetIfPossible()
    {
        attackObject.SetActive(true);
        distanceTraveled = 0;
        rb.linearVelocity = new Vector2(enemy.EnemyMovement.IsFacingRight ? attackSpeed : -attackSpeed, 0);

        while (distanceTraveled < attackDistance)
        {
            distanceTraveled += enemy.EnemyMovement.Speed * Time.deltaTime;
            if (!enemy.EnemyMovement.IsGroundInFront())
                distanceTraveled = attackDistance;
            yield return null;
        }
        attackObject.SetActive(false);
        distanceTraveled = 0;
        rb.linearVelocity = Vector2.zero;
    }
    #endregion
}
