using Unity.Netcode;
using UnityEngine;
public class FlameInteractions : BulletInteractions
{
    #region Functions
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isOffline || (IsOwner && !bulletCollided))
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

    protected override void ManagePlayerCollision(Player player)
    {

    }

    protected override void ManageFloorCollision(Collider2D collision)
    {

    }

    protected override void ManageEnemyCollision(Enemy enemy)
    {

    }

    void ManageEnemyCollisionStay(Enemy enemy)
    {
        if (isOffline || (IsOwner && !bulletCollided))
        {
            if (isOffline)
            {
                ManageOfflineEnemyCollisionStay(enemy);
            }
            else
            {
                ManageOnlineEnemyCollisionStay(enemy);
            }
        }
    }

    void ManageOnlineEnemyCollisionStay(Enemy enemy)
    {
        if (!canAttackEnemies) return;
        if (enemy == ownerEnemy) return;
        clientId[0] = enemy.GetComponent<NetworkObject>().OwnerClientId;
        clientRpcParams.Send.TargetClientIds = clientId;
        enemy.DecrementLifeClientRpc(damage * Time.deltaTime, clientRpcParams);
        //if (enemy.Health.CanBurn)
        //    enemy.StartBurningServerRpc(); // <-- Cambiado aquí
    }

    void ManageOfflineEnemyCollisionStay(Enemy enemy)
    {
        if (!canAttackEnemies) return;
        if (enemy == ownerEnemy) return;
        enemy.DecrementLife(damage * Time.deltaTime);
        if (enemy.Health.CanBurn)
            enemy.Burning.StartBurning();
    }

    void ManagePlayerCollisionStay(Player player)
    {
        if (isOffline || (IsOwner && !bulletCollided))
        {
            if (isOffline)
            {
                ManageOfflinePlayerCollisionStay(player);
            }
            else
            {
                ManageOnlinePlayerCollisionStay(player);
            }
        }
    }

    void ManageOfflinePlayerCollisionStay(Player player)
    {
        if (!canAttackPlayers) return;
        if (player == ownerPlayer) return;
        player.DecrementLife(damage * Time.deltaTime);
        if (player.Health.CanBurn)
            player.Burning.StartBurning();
    }

    void ManageOnlinePlayerCollisionStay(Player player)
    {
        if (!canAttackPlayers) return;
        if (player == ownerPlayer) return;
        clientId[0] = player.GetComponent<NetworkObject>().OwnerClientId;
        Debug.Log("Flame hit player " + clientId[0]);
        clientRpcParams.Send.TargetClientIds = clientId;
        player.DecrementLifeClientRpc(damage * Time.deltaTime, clientRpcParams);
        //if (player.Health.CanBurn)
        //    player.StartBurningServerRpc(); // <-- Cambiado aquí
    }
    #endregion
}
