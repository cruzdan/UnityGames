using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerBurn : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private BurnStatus burnStatus;
    [SerializeField] private Player player;
    [SerializeField] private float burningDamagePerSecond = 5f;
    #endregion
    #region Network
    protected ClientRpcParams clientRpcParams;
    protected readonly ulong[] clientId = new ulong[1];
    #endregion
    #region Actions
    public static Action<Player> OnPlayerKilled;
    #endregion
    #region Functions
    private void Start()
    {
        if (GameNetwork.Instance.IsOnline && !IsServer) { enabled = false; return; }
        burnStatus.OnBurn += HandleBurn;
    }

    void HandleBurn()
    {
        if (GameNetwork.Instance.IsOnline)
            HandleBurnServerRpc();
        else
            HandleBurnLocal();
    }

    [ServerRpc(RequireOwnership = false)]
    void HandleBurnServerRpc()
    {
        clientId[0] = player.NetworkObject.OwnerClientId;
        clientRpcParams.Send.TargetClientIds = clientId;
        if (!player.dead.Value && !player.Invincible.Value)
        {
            HandlePlayerKilledIfPossible(player, burningDamagePerSecond * Time.deltaTime);
            player.DecrementLife(burningDamagePerSecond * Time.deltaTime);
        }
    }

    void HandleBurnLocal()
    {
        if (!player.dead.Value && !player.Invincible.Value)
        {
            HandlePlayerKilledIfPossible(player, burningDamagePerSecond * Time.deltaTime);
            player.DecrementLife(burningDamagePerSecond * Time.deltaTime);
        }
    }

    void HandlePlayerKilledIfPossible(Player player, float damage)
    {
        if (player.CurrentLife.Value - damage <= 0)
        {
            if (player.dead.Value) return;
            if (burnStatus.AttackingPlayer != null)
            {
                // Make the player dead to avoid multiple death triggers
                player.dead.Value = true;
                OnPlayerKilled?.Invoke(burnStatus.AttackingPlayer);
            }
        }
    }
    #endregion
}
