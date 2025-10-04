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
    [SerializeField] private FasterEnemyInteractions fasterEnemyInteractions;
    #endregion
    #region Private Variables
    private float distanceTraveled;
    private Coroutine attackCoroutine;
    #endregion
    #region Functions
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
        DisposeAttackCoroutine();
    }

    IEnumerator AttackTargetIfPossible()
    {
        attackObject.SetActive(true);
        fasterEnemyInteractions.ClearPlayersHitted();
        distanceTraveled = 0;
        rb.velocity = new Vector2(enemy.EnemyMovement.IsFacingRight ? attackSpeed : -attackSpeed, 0);

        while (distanceTraveled < attackDistance)
        {
            distanceTraveled += enemy.EnemyMovement.Speed * Time.deltaTime;
            if (!enemy.EnemyMovement.IsGroundInFront())
                distanceTraveled = attackDistance;
            yield return null;
        }
        attackObject.SetActive(false);
        distanceTraveled = 0;
        rb.velocity = Vector2.zero;
    }

    void DisposeAttackCoroutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
    #endregion
}
