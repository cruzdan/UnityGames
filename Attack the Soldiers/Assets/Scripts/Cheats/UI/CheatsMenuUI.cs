using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

//Class to manage the cheats menu UI for players in a networked game.
public class CheatsMenuUI :  NetworkBehaviour
{
    #region General
    [Header("General")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private BoxManager boxManager;
    #endregion
    #region Menus
    [Header("Menus")]
    [SerializeField] private GameObject cheatCanvasObject;
    [SerializeField] private GameObject mainPanelObject;
    [SerializeField] private GameObject enemyPanelObject;
    [SerializeField] private GameObject playerPanelObject;
    [SerializeField] private GameObject boxPanelObject;
    #endregion
    #region Main Panel
    [Header("Main Panel")]
    [SerializeField] private Button enemyButton;
    [SerializeField] private Button playerButton;
    [SerializeField] private Button boxButton;
    [SerializeField] private Button mainPanelCloseButton;
    #endregion
    #region Enemy Panel
    [Header("Enemy Panel")]
    [SerializeField] private Toggle overrideEnemySpawnToggle;
    [SerializeField] private TMP_Dropdown overrideEnemySpawnDropdown;
    [SerializeField] private Button enemyPanelBackButton;
    #endregion
    #region Player Panel
    [Header("Player Panel")]
    [SerializeField] private Button playerPanelBackButton;
    #endregion
    #region Box Panel
    [Header("Box Panel")]
    [SerializeField] private Toggle overrideBoxSpawnToggle;
    [SerializeField] private TMP_Dropdown overrideBoxSpawnDropdown;
    [SerializeField] private Toggle overrideBoxWeaponToggle;
    [SerializeField] private TMP_Dropdown overrideBoxWeaponDropdown;
    [SerializeField] private Button boxPanelBackButton;
    #endregion
    #region Actions
    #region Enemy Panel
    public Action<bool> OnOverrideEnemySpawnAction;
    public Action<int> OnOverrideEnemySpawnDropboxAction;
    #endregion
    #region Box Panel
    public Action<bool> OnOverrideBoxTypeChangedAction;
    public Action<int> OnOverrideBoxSpawnDropdownChanedAction;
    public Action<bool> OnOverrideBoxWeaponToggleChangedAction;
    public Action<int> OnOverrideBoxWeaponDropdownChangedAction;
    #endregion
    #endregion
    #region Functions
    public override void OnNetworkSpawn()
    {
        if (!GameNetwork.Instance.IsOnline)
        {
            enabled = false;
            return;
        }
        InitializeCheatsMenuWithCurrentInfo();
        AddMainPanelEvents();
        AddEnemyPanelEvents();
        AddPlayerPanelEvents();
        AddBoxPanelEvents();
    }

    void InitializeCheatsMenuWithCurrentInfo()
    {
        SetCurrentEnemyInfoServerRpc();
        SetCurrentBoxInfoServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetCurrentEnemyInfoServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong[] clientId = new ulong[1];
        clientId[0] = serverRpcParams.Receive.SenderClientId;
        ClientRpcParams clientRpcParams1 = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = clientId
            }
        };
        SetCurrentEnemyInfoClientRpc(enemyManager.overrideEnemySpawn, enemyManager.overrideEnemyIndexToSpawn, clientRpcParams1);
    }

    [ClientRpc]
    public void SetCurrentEnemyInfoClientRpc(bool overrideEnemySpawnToggleValue, int overrideEnemySpawnDropdownValue, ClientRpcParams clientRpcParams = default)
    {
        overrideEnemySpawnToggle.SetIsOnWithoutNotify(overrideEnemySpawnToggleValue);
        overrideEnemySpawnDropdown.SetValueWithoutNotify(overrideEnemySpawnDropdownValue);
    }

    [ServerRpc(RequireOwnership = false)]
    void SetCurrentBoxInfoServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong[] clientId = new ulong[1];
        clientId[0] = serverRpcParams.Receive.SenderClientId;
        ClientRpcParams clientRpcParams1 = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = clientId
            }
        };
        SetCurrentBoxInfoClientRpc(boxManager.overrideBoxType, (int)boxManager.boxTypeOverrideValue,
            boxManager.overrideWeapon, (int)boxManager.weaponOverrideValue, clientRpcParams1);
    }

    [ClientRpc]
    void SetCurrentBoxInfoClientRpc(bool overrideBoxSpawnToggleValue, int overrideBoxSpawnDropdownValue, 
        bool overrideBoxWeaponToggleValue, int overrideBoxWeaponDropdownValue, ClientRpcParams clientRpcParams = default)
    {
        overrideBoxSpawnToggle.SetIsOnWithoutNotify(overrideBoxSpawnToggleValue);
        overrideBoxSpawnDropdown.SetValueWithoutNotify(overrideBoxSpawnDropdownValue);
        overrideBoxWeaponToggle.SetIsOnWithoutNotify(overrideBoxWeaponToggleValue);
        overrideBoxWeaponDropdown.SetValueWithoutNotify(overrideBoxWeaponDropdownValue);
    }

    void AddMainPanelEvents()
    {
        enemyButton.onClick.AddListener(OpenEnemyPanel);
        playerButton.onClick.AddListener(OpenPlayerPanel);
        boxButton.onClick.AddListener(OpenBoxPanel);
        mainPanelCloseButton.onClick.AddListener(CloseCheatsMenu);
    }

    void CloseAllPanels()
    {
        mainPanelObject.SetActive(false);
        enemyPanelObject.SetActive(false);
        playerPanelObject.SetActive(false);
        boxPanelObject.SetActive(false);
    }

    void OpenEnemyPanel()
    {
        CloseAllPanels();
        enemyPanelObject.SetActive(true);
    }

    void OpenPlayerPanel()
    {
        CloseAllPanels();
        playerPanelObject.SetActive(true);
    }

    void OpenBoxPanel()
    {
        CloseAllPanels();
        boxPanelObject.SetActive(true);
    }

    public void CloseCheatsMenu()
    {
        cheatCanvasObject.SetActive(false);
    }

    void AddEnemyPanelEvents()
    {
        enemyPanelBackButton.onClick.AddListener(OpenMainPanel);
        overrideEnemySpawnToggle.onValueChanged.AddListener(OnOverrideEnemySpawn);
        overrideEnemySpawnDropdown.onValueChanged.AddListener(OnOverrideEnemySpawnDropbox);
    }

    void OpenMainPanel()
    {
        CloseAllPanels();
        mainPanelObject.SetActive(true);
    }

    void AddPlayerPanelEvents()
    {
        playerPanelBackButton.onClick.AddListener(OpenMainPanel);
    }

    void AddBoxPanelEvents()
    {
        boxPanelBackButton.onClick.AddListener(OpenMainPanel);
        overrideBoxSpawnToggle.onValueChanged.AddListener(OnOverrideBoxTypeChanged);
        overrideBoxSpawnDropdown.onValueChanged.AddListener(OnOverrideBoxSpawnDropdownChaned);
        overrideBoxWeaponToggle.onValueChanged.AddListener(OnOverrideBoxWeaponToggleChanged);
        overrideBoxWeaponDropdown.onValueChanged.AddListener(OnOverrideBoxWeaponDropdownChanged);
    }

    void OnOverrideEnemySpawn(bool value)
    {
        OnOverrideEnemySpawnAction?.Invoke(value);
    }

    void OnOverrideEnemySpawnDropbox(int index)
    {
        OnOverrideEnemySpawnDropboxAction?.Invoke(index);
    }

    void OnOverrideBoxTypeChanged(bool value)
    {
        OnOverrideBoxTypeChangedAction?.Invoke(value);
    }

    void OnOverrideBoxSpawnDropdownChaned(int index)
    {
        OnOverrideBoxSpawnDropdownChanedAction?.Invoke(index);
    }

    void OnOverrideBoxWeaponToggleChanged(bool value)
    {
        OnOverrideBoxWeaponToggleChangedAction?.Invoke(value);
    }

    void OnOverrideBoxWeaponDropdownChanged(int index)
    {
        OnOverrideBoxWeaponDropdownChangedAction?.Invoke(index);
    }

    public void OnCheatsOpen_Close()
    {
        cheatCanvasObject.SetActive(!cheatCanvasObject.activeInHierarchy);
        if (cheatCanvasObject.activeInHierarchy)
        {
            OpenMainPanel();
        }
    }
    #endregion
}

