using Unity.Netcode;
using UnityEngine;

public class EnemyBurn : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private BurnStatus burnStatus;
    [SerializeField] private Enemy enemy;
    [SerializeField] private float burningDamagePerSecond = 5f;
    #endregion
    #region Network
    protected ClientRpcParams clientRpcParams;
    protected readonly ulong[] clientId = new ulong[1];
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
            enemy.DecrementLife(burningDamagePerSecond * Time.deltaTime);
    }

    [ServerRpc(RequireOwnership = false)]
    void HandleBurnServerRpc()
    {
        clientId[0] = enemy.NetworkObject.OwnerClientId;
        clientRpcParams.Send.TargetClientIds = clientId;
        enemy.DecrementLifeClientRpc(burningDamagePerSecond * Time.deltaTime, clientRpcParams);
    }
    #endregion
}
