using System.Collections;
using UnityEngine;
public class BarrelThrowerAttack : EnemyAttack
{
    #region Serialized Variables
    [SerializeField] private float attackAnimationDuration = 0.2f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private Shoot shoot;
    #endregion
    #region Private Variables
    private Coroutine attackCoroutine;
    #endregion
    #region Functions
    private void Start()
    {
        shoot.SetCurrentWeapon(Weapon.Barrel, 0);
        shoot.Infinite = true;
        shoot.OverrideDamage = true;
        shoot.DamageOverride = damage;
        shoot.OwnerEnemy = enemy;
        shoot.OwnerPlayer = null;
        shoot.CanAttackPlayers = true;
        shoot.CanAttackEnemies = false;
    }

    public override void SetDamage(int damage)
    {
        base.SetDamage(damage);
        shoot.DamageOverride = damage;
    }

    public override void StartAttack()
    {
        attackCoroutine = StartCoroutine(BarrelAttack());
    }
    public override void Attack()
    {
        enemy.EnemyMovement.FlipEnemyDirectionIfPossible();
    }
    IEnumerator BarrelAttack()
    {
        while (!TargetIsOutOfRange())
        {
            //enemy.Animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimationDuration);
            //attackObject.SetActive(true);
            AttackTargetIfPossible();
            yield return new WaitForSeconds(attackDelay);
            //attackObject.SetActive(false);
            //enemy.Animator.SetTrigger("AttackCooldown");
            yield return new WaitForSeconds(attackCooldown);
        }
        enemy.StartChase();
        CoroutineExtensions.StopCoroutineSafe(this, ref attackCoroutine);
    }

    void AttackTargetIfPossible()
    {
        shoot.ShootCurrentWeapon();
    }
    #endregion
}
