using Unity.Netcode;
using UnityEngine;
//Class in charge of general enemy attack behavior
public class EnemyAttack : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected int damage;
    [SerializeField] protected int initialDamage;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCooldown;
    #endregion
    #region Private Variables
    protected float timer;
    #endregion
    #region Public Properties
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public int Damage { get => damage; set => damage = value; }
    public int InitialDamage { get => initialDamage; set => initialDamage = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    #endregion
    #region Functions

    public virtual void SetDamage(int damage)
    {
        this.damage = damage;
    }

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
    #endregion
}
