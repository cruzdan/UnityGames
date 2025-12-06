using UnityEngine;
public class FlameInteractions : BulletInteractions
{
    #region Functions
    protected override void ManagePlayerCollision(Player player) { }

    protected override void ManageFloorCollision(Collider2D collision) { }

    protected override void ManageEnemyCollision(Enemy enemy) { }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameNetwork.IsOwnerOfflineOrOnline(NetworkObject) && !bulletCollided)
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    ManagePlayerCollisionStay(collision.GetComponent<Player>());
                    break;
                case "Enemy":
                    ManageEnemyCollisionStay(collision.GetComponent<Enemy>());
                    break;
            }
        }
    }

    void ManagePlayerCollisionStay(Player player)
    {
        if (GameNetwork.IsOwnerOfflineOrOnline(NetworkObject) && !bulletCollided)
        {
            if (!canAttackPlayers) return;
            if (player == ownerPlayer) return;
            if (!player.dead.Value && !player.Invincible.Value)
            {
                HandlePlayerKilledIfPossible(player, damage * Time.deltaTime);
                player.DecrementLife(damage * Time.deltaTime);
                BurnStatus burnStatus = player.GetComponent<BurnStatus>();
                burnStatus.ApplyBurn();
                burnStatus.AttackingPlayer = OwnerPlayer;
            }
        }
    }

    void ManageEnemyCollisionStay(Enemy enemy)
    {
        if (!canAttackEnemies) return;
        if (enemy == ownerEnemy) return;
        if (enemy.Health.CurrentLife <= 0) return;
        if (GameNetwork.Instance.IsOnline)
            DecrementServerEnemyLife(enemy, damage * Time.deltaTime);
        else
            enemy.DecrementLife(damage * Time.deltaTime);
        enemy.GetComponent<BurnStatus>().ApplyBurn();
    }
    #endregion
}
