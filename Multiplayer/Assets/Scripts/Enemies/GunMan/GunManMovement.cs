using UnityEngine;

//Class that handles the movement of the GunMan enemy
public class GunManMovement : EnemyMovement
{
    #region Functions
    public override void Chase()
    {
        if (enemy.PlayerTarget == null) return;
        FlipEnemyDirectionIfPossible();
        JumpIfPossible();
        FollowTarget();
        PassToAttackIfPossible();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
    }
    #endregion
}
