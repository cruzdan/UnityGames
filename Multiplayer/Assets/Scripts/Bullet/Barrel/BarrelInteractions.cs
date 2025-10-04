using Unity.Netcode;
using UnityEngine;
public class BarrelInteractions : BulletInteractions
{
    #region Functions
    protected override void ManageOfflinePlayerCollision(Player player)
    {
        if (!canAttackPlayers) return;
        if (player == ownerPlayer) return;
        player.DecrementLife(damage);
        ObjectPool.Singleton.ReturnObject(gameObject, "Barrel");
    }

    protected override void ManageOnlinePlayerCollision(Player player)
    {
        if (!canAttackPlayers) return;
        if (player == ownerPlayer) return;
        clientId[0] = player.GetComponent<NetworkObject>().OwnerClientId;
        clientRpcParams.Send.TargetClientIds = clientId;
        player.DecrementLifeClientRpc(damage, clientRpcParams);
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Barrel Online");
    }

    protected override void ManageFloorCollision(Collider2D collision)
    {

    }

    protected override void ManageOfflineEnemyCollision(Enemy enemy, string bulletName)
    {
        base.ManageOfflineEnemyCollision(enemy, "Barrel");
    }

    protected override void ManageOnlineEnemyCollision(Enemy enemy, string bulletName)
    {
        base.ManageOnlineEnemyCollision(enemy, "Barrel Online");
    }
    #endregion
}
