using UnityEngine;
using Unity.Netcode;
public class BulletInteractions : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] protected bool isOffline = false;
    [SerializeField] protected float damage;
    [Header("Attack Settings")]
    [SerializeField] protected bool canAttackPlayers;
    [SerializeField] protected Enemy ownerEnemy;
    [SerializeField] protected bool canAttackEnemies;
    [SerializeField] protected Player ownerPlayer;
    protected bool bulletCollided = false;
    #endregion
    #region Network
    protected ClientRpcParams clientRpcParams;
    protected readonly ulong[] clientId = new ulong[1];
    #endregion
    #region Public Properties
    public bool CanAttackPlayers { get { return canAttackPlayers; } set => canAttackPlayers = value; }
    public bool CanAttackEnemies { get { return canAttackEnemies; } set => canAttackEnemies = value; }
    public Enemy OwnerEnemy { get { return ownerEnemy; } set => ownerEnemy = value; }
    public Player OwnerPlayer { get { return ownerPlayer; } set => ownerPlayer = value; }
    #endregion
    #region Functions
    public bool BulletCollided { get { return bulletCollided; } set => bulletCollided = value; }
    public void SetDamage(float value) { damage = value; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOffline || (IsOwner && !bulletCollided))
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    ManagePlayerCollision(collision.GetComponent<Player>());
                    break;
                case "Floor":
                    ManageFloorCollision(collision);
                    break;
                case "Enemy":
                    ManageEnemyCollision(collision.GetComponent<Enemy>());
                    break;
            }
        }
    }

    protected virtual void ManagePlayerCollision(Player player)
    {
        if (!isOffline)
        {
            ManageOnlinePlayerCollision(player);
        }
        else
        {
            ManageOfflinePlayerCollision(player);
        }
    }

    protected virtual void ManageOfflinePlayerCollision(Player player)
    {
        if (!canAttackPlayers) return;
        if (player == ownerPlayer) return;
        ObjectPool.Singleton.ReturnObject(gameObject, "Offline Bullet");
        player.DecrementLife(damage);
        bulletCollided = true;
    }

    protected virtual void ManageOnlinePlayerCollision(Player player)
    {
        if (!canAttackPlayers) return;
        if (player == ownerPlayer) return;
        clientId[0] = player.GetComponent<NetworkObject>().OwnerClientId;
        clientRpcParams.Send.TargetClientIds = clientId;
        player.DecrementLifeClientRpc(damage, clientRpcParams);
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Bullet");
        bulletCollided = true;
    }

    virtual protected void ManageFloorCollision(Collider2D collision)
    {
        if (!isOffline)
        {
            ManageOnlineFloorCollision(collision);
        }
        else
        {
            ManageOfflineFloorCollision(collision);
        }
    }

    protected virtual void ManageOfflineFloorCollision(Collider2D collider)
    {
        ObjectPool.Singleton.ReturnObject(gameObject, "Offline Bullet");
    }

    protected virtual void ManageOnlineFloorCollision(Collider2D collider)
    {
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Bullet");
    }

    [ClientRpc]
    public void ChangeColorClientRpc(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void ChangeColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    virtual protected void ManageEnemyCollision(Enemy enemy)
    {
        if (!isOffline)
        {
            ManageOnlineEnemyCollision(enemy, "Bullet");
        }
        else
        {
            ManageOfflineEnemyCollision(enemy, "Offline Bullet");
        }
    }

    protected virtual void ManageOnlineEnemyCollision(Enemy enemy, string bulletName)
    {
        if (!canAttackEnemies) return;
        if (enemy == ownerEnemy) return;
        clientId[0] = enemy.GetComponent<NetworkObject>().OwnerClientId;
        clientRpcParams.Send.TargetClientIds = clientId;
        //enemy.DecrementLifeClientRpc(damage, clientRpcParams);
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), bulletName);
        bulletCollided = true;
    }

    protected virtual void ManageOfflineEnemyCollision(Enemy enemy, string bulletName)
    {
        if (!canAttackEnemies) return;
        if (enemy == ownerEnemy) return;
        if (bulletCollided) return;
        enemy.DecrementLife(damage);
        ObjectPool.Singleton.ReturnObject(gameObject, bulletName);
        bulletCollided = true;
    }
    #endregion
}