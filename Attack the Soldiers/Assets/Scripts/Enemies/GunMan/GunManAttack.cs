using UnityEngine;
public class GunManAttack : EnemyAttack
{
    #region Serialized Variables
    [SerializeField] private Shoot shoot;
    #endregion
    #region Functions
    private void Start()
    {
        shoot.OverrideDamage = true;
        shoot.DamageOverride = damage;
        shoot.CanAttackEnemies = false;
        shoot.CanAttackPlayers = true;
        shoot.OwnerPlayer = null;
        shoot.OwnerEnemy = enemy;
    }

    public override void SetDamage(int damage)
    {
        base.SetDamage(damage);
        shoot.DamageOverride = damage;
    }

    public override void Attack()
    {
        enemy.EnemyMovement.FlipEnemyDirectionIfPossible();
        if (TargetIsOutOfRange())
        {
            enemy.StartChase();
            return;
        }
        HandleShoot();
    }

    void HandleShoot()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            shoot.ShootCurrentWeapon();
            timer = attackCooldown;
        }
    }
    #endregion
}
