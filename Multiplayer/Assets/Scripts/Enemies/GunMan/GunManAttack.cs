using UnityEngine;
public class GunManAttack : EnemyAttack
{
    [SerializeField] private Shoot shoot;
    private void Start()
    {
        shoot.OverrideDamage = true;
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

    public override void StartAttack()
    {
        //timer = 0;
    }

    
}
