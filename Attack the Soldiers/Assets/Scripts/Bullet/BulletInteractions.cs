using UnityEngine;
using Unity.Netcode;
using System;
public class BulletInteractions : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] protected float damage;
    [Header("Attack Settings")]
    [SerializeField] protected bool canAttackPlayers;
    [SerializeField] protected Enemy ownerEnemy;
    [SerializeField] protected bool canAttackEnemies;
    [SerializeField] protected Player ownerPlayer;
    [SerializeField] private string poolTag;
    protected bool bulletCollided = false;
    #endregion
    #region Private Variables
    private SpriteRenderer spriteRenderer;
    #endregion
    #region Network
    protected ClientRpcParams clientRpcParams;
    protected readonly ulong[] clientId = new ulong[1];
    public NetworkVariable<Color> networkColor = new NetworkVariable<Color>(Color.white);
    #endregion
    #region Public Properties
    public bool CanAttackPlayers { get { return canAttackPlayers; } set => canAttackPlayers = value; }
    public bool CanAttackEnemies { get { return canAttackEnemies; } set => canAttackEnemies = value; }
    public Enemy OwnerEnemy { get { return ownerEnemy; } set => ownerEnemy = value; }
    public Player OwnerPlayer { get { return ownerPlayer; } set => ownerPlayer = value; }
    public string PoolTag { get { return poolTag; } set { poolTag = value; } }
    #endregion
    #region Actions
    public static Action<Player> OnPlayerKilled;
    #endregion
    #region Functions
    public bool BulletCollided { get { return bulletCollided; } set => bulletCollided = value; }
    public void SetDamage(float value) { damage = value; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            spriteRenderer.color = networkColor.Value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameNetwork.IsOwnerOfflineOrOnline(NetworkObject) && !bulletCollided)
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
        if (!canAttackPlayers) return;
        if (player == ownerPlayer) return;
        bulletCollided = true;
        if (!player.dead.Value && !player.Invincible.Value)
        {
            HandlePlayerKilledIfPossible(player, damage);
            player.DecrementLife(damage);
        }
        GameNetwork.Instance.Despawn(NetworkObject, poolTag);
    }

    protected void HandlePlayerKilledIfPossible(Player player, float nextDamage)
    {
        if (player.CurrentLife.Value - nextDamage <= 0)
        {
            if (player.dead.Value) return;
            if (OwnerPlayer != null)
            {
                // Make the player dead to avoid multiple death triggers
                player.dead.Value = true;
                OnPlayerKilled?.Invoke(OwnerPlayer);
            }
        }
    }

    public void DecrementServerPlayerLife(Player player, float damageToDecrement)
    {
        clientId[0] = player.NetworkObject.OwnerClientId;
        clientRpcParams.Send.TargetClientIds = clientId;
        player.DecrementLifeClientRpc(damageToDecrement, clientRpcParams);
    }

    public void DecrementServerEnemyLife(Enemy enemy, float damageToDecrement)
    {
        clientId[0] = enemy.EnemyNetworkObject.OwnerClientId;
        clientRpcParams.Send.TargetClientIds = clientId;
        enemy.DecrementLifeClientRpc(damageToDecrement, clientRpcParams);
    }

    virtual protected void ManageFloorCollision(Collider2D collision)
    {
        GameNetwork.Instance.Despawn(NetworkObject, poolTag);
    }

    [ClientRpc]
    public void ChangeColorClientRpc(Color color)
    {
        ChangeColor(color);
    }

    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    virtual protected void ManageEnemyCollision(Enemy enemy)
    {
        if (!canAttackEnemies) return;
        if (enemy == ownerEnemy) return;
        if (enemy.Health.CurrentLife <= 0) return;
        bulletCollided = true;
        GameNetwork.Instance.Despawn(NetworkObject, poolTag);
        if (GameNetwork.Instance.IsOnline)
            DecrementServerEnemyLife(enemy, damage);
        else
            enemy.DecrementLife(damage);
    }
    #endregion
}