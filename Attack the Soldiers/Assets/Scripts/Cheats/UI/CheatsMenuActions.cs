using Unity.Netcode;
using UnityEngine;

// Class in charge of handling the actions from the Cheats Menu UI
public class CheatsMenuActions : NetworkBehaviour
{
    #region Serialized Variables
    [SerializeField] private CheatsMenuUI cheatsMenuUI;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private ObjectContainerUI playerContainerUI;
    [SerializeField] private BoxManager boxManager;
    #endregion
    #region Network
    private ClientRpcParams clientRpcParams;
    private readonly ulong[] clientId = new ulong[1];
    #endregion
    #region Functions
    public override void OnNetworkSpawn()
    {
        if (!GameNetwork.Instance.IsOnline) return;
        AddEnemyActions();
        AddPlayerActions();
        AddBoxActions();
    }
    #region Enemy
    void AddEnemyActions()
    {
        cheatsMenuUI.OnOverrideEnemySpawnAction += OnOverrideEnemySpawn;
        cheatsMenuUI.OnOverrideEnemySpawnDropboxAction += OnOverrideEnemySpawnDropdown;
    }

    void OnOverrideEnemySpawn(bool value)
    {
        OnOverrideEnemySpawnServerRpc(value);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnOverrideEnemySpawnServerRpc(bool value)
    {
        enemyManager.overrideEnemySpawn = value;
    }

    void OnOverrideEnemySpawnDropdown(int index)
    {
        OnOverrideEnemySpawnDropdownServerRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnOverrideEnemySpawnDropdownServerRpc(int index)
    {
        enemyManager.overrideEnemyIndexToSpawn = index;
    }
    #endregion
    #region Player
    void AddPlayerActions()
    {
        int total = playerContainerUI.ObjectsInUI.Length;
        for (int i = 0; i < total; i++)
        {
            PlayerCheatInfo playerCheatInfo = playerContainerUI.ObjectsInUI[i].GetComponent<PlayerCheatInfo>();
            playerCheatInfo.OnPlayerWeaponChangedAction += OnPlayerWeaponChangedAction;
            playerCheatInfo.OnPlayerSpeedChangedAction += OnPlayerSpeedChangedAction;
            playerCheatInfo.OnStaminaToggleChangedAction += OnStaminaToggleChangedAction;
            playerCheatInfo.OnAmmoToggleChangedAction += OnAmmoToggleChangedAction;
            playerCheatInfo.OnPlayerJumpChangedAction += OnPlayerJumpChangedAction;
            playerCheatInfo.OnPlayerTeleportToMeAction += OnPlayerTeleportToMeAction;
        }
    }

    void OnPlayerWeaponChangedAction(int value, ulong playerID)
    {
        OnPlayerWeaponChangedActionServerRpc(value, playerID);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnPlayerWeaponChangedActionServerRpc(int valueIndex, ulong playerID)
    {
        Player player = GetPlayerByID(playerID);
        if (player != null)
        {
            clientId[0] = player.OwnerClientId;
            clientRpcParams.Send.TargetClientIds = clientId;
            player.GetComponent<Shoot>().SetCurrentWeaponWithSOClientRpc((Weapon)valueIndex, clientRpcParams);
        }
    }

    void OnPlayerSpeedChangedAction(float value, ulong playerID)
    {
        OnPlayerSpeedChangedActionServerRpc(value, playerID);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnPlayerSpeedChangedActionServerRpc(float value, ulong playerID)
    {
        Player player = GetPlayerByID(playerID);
        if (player != null)
        {
            clientId[0] = player.OwnerClientId;
            clientRpcParams.Send.TargetClientIds = clientId;
            player.GetComponent<PlayerMovement>().SetWalkSpeedClientRpc(value * 15, clientRpcParams);
        }
    }

    void OnStaminaToggleChangedAction(bool value, ulong playerID)
    {
        OnStaminaToggleChangedActionServerRpc(value, playerID);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnStaminaToggleChangedActionServerRpc(bool value, ulong playerID)
    {
        Player player = GetPlayerByID(playerID);
        if (player != null)
        {
            clientId[0] = player.OwnerClientId;
            clientRpcParams.Send.TargetClientIds = clientId;
            player.GetComponent<PlayerMovement>().SetUseStaminaClientRpc(value, clientRpcParams);
        }
    }

    void OnAmmoToggleChangedAction(bool value, ulong playerID)
    {
        OnAmmoToggleChangedActionServerRpc(value, playerID);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnAmmoToggleChangedActionServerRpc(bool value, ulong playerID)
    {
        Player player = GetPlayerByID(playerID);
        if (player != null)
        {
            clientId[0] = player.OwnerClientId;
            clientRpcParams.Send.TargetClientIds = clientId;
            player.GetComponent<Shoot>().SetInfiniteAmmoClientRpc(value, clientRpcParams);
        }
    }

    void OnPlayerJumpChangedAction(float value, ulong playerID)
    {
        OnPlayerJumpChangedActionServerRpc(value, playerID);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnPlayerJumpChangedActionServerRpc(float value, ulong playerID)
    {
        Player player = GetPlayerByID(playerID);
        if (player != null)
        {
            clientId[0] = player.OwnerClientId;
            clientRpcParams.Send.TargetClientIds = clientId;
            player.GetComponent<PlayerMovement>().SetJumpForceClientRpc(value * 45, clientRpcParams);
        }
    }


    void OnPlayerTeleportToMeAction(ulong playerID)
    {
        OnPlayerTeleportToMeActionServerRpc(playerID);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnPlayerTeleportToMeActionServerRpc(ulong playerID)
    {
        Player player = GetPlayerByID(playerID);
        if (player != null)
        {
            clientId[0] = player.OwnerClientId;
            clientRpcParams.Send.TargetClientIds = clientId;
            player.GetComponent<PlayerMovement>().TeleportToPositionClientRpc(PlayerManager.Instance.Players[0].transform.position, clientRpcParams);
        }
    }

    Player GetPlayerByID(ulong playerID)
    {
        foreach (var player in PlayerManager.Instance.Players)
        {
            if (player.NetworkObjectId == playerID)
            {
                return player;
            }
        }
        return null;
    }
    #endregion
    #region Box
    void AddBoxActions()
    {
        cheatsMenuUI.OnOverrideBoxTypeChangedAction += OnOverrideBoxTypeChanged;
        cheatsMenuUI.OnOverrideBoxSpawnDropdownChanedAction += OnOverrideBoxSpawnDropdownChanged;
        cheatsMenuUI.OnOverrideBoxWeaponToggleChangedAction += OnOverrideBoxWeaponToggleChanged;
        cheatsMenuUI.OnOverrideBoxWeaponDropdownChangedAction += OnOverrideBoxWeaponDropdownChanged;
    }

    void OnOverrideBoxTypeChanged(bool value)
    {
        OnOverrideBoxTypeChangedServerRpc(value);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnOverrideBoxTypeChangedServerRpc(bool value)
    {
        boxManager.overrideBoxType = value;
    }

    void OnOverrideBoxSpawnDropdownChanged(int index)
    {
        OnOverrideBoxSpawnDropdownChangedServerRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnOverrideBoxSpawnDropdownChangedServerRpc(int index)
    {
        boxManager.boxTypeOverrideValue = (BoxType)index;
    }

    void OnOverrideBoxWeaponToggleChanged(bool value)
    {
        OnOverrideBoxWeaponToggleChangedServerRpc(value);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnOverrideBoxWeaponToggleChangedServerRpc(bool value)
    {
        boxManager.overrideWeapon = value;
    }

    void OnOverrideBoxWeaponDropdownChanged(int index)
    {
        OnOverrideBoxWeaponDropdownChangedServerRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    void OnOverrideBoxWeaponDropdownChangedServerRpc(int index)
    {
        boxManager.weaponOverrideValue = (Weapon)index;
    }
    #endregion
    #endregion
}
