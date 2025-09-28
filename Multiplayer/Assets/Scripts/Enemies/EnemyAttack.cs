using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCooldown;
    protected float timer;
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public virtual void Attack()
    {

    }

    public virtual void StartAttack()
    {
    }

    protected bool TargetIsOutOfRange()
    {
        if (enemy.PlayerTarget == null) return true;
        float distance = Vector2.Distance(transform.position, enemy.PlayerTarget.transform.position);
        if (Mathf.Abs(distance) > AttackRange)
            return true;
        return false;
    }

    protected bool TargetIsNearby()
    {
        float distance = Vector2.Distance(transform.position, enemy.PlayerTarget.transform.position);
        return Mathf.Abs(distance) <= enemy.EnemyAttack.AttackRange;
    }

    protected bool TargetIsNearbyInX()
    {
        return Mathf.Abs(transform.position.x - enemy.PlayerTarget.transform.position.x) <=
            enemy.EnemyAttack.AttackRange;
    }
}
