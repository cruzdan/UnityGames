public class KnockoutMovement : EnemyMovement
{
    public override void Chase()
    {
        if (enemy.PlayerTarget == null) return;
        FlipEnemyDirectionIfPossible();
        JumpIfPossible();
        FollowTarget();
        PassToAttackIfPossible();
    }
}
