using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region Menus
    [Header("Menus")]
    [SerializeField] public GameObject UICanvas;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject multiplayerModesMenu;
    [SerializeField] private GameObject connectionMenu;
    [SerializeField] private GameObject upgradesMenu;
    #endregion
    #region Main Menu
    [Header("Main Menu")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button upgradesButton;
    [SerializeField] private Button exitButton;
    #endregion
    #region Play Menu
    [Header("Play Menu")]
    [SerializeField] private Button onlineButton;
    [SerializeField] private Button offlineButton;
    [SerializeField] private Button playMenuBackButton;
    #endregion
    #region Multiplayer Modes Menu
    [Header("Multiplayer Modes Menu")]
    [SerializeField] private Button pvpModeButton;
    [SerializeField] private Button coopModeButton;
    [SerializeField] private Button multiplayerModesMenuBackButton;
    #endregion
    #region Connection Menu
    [Header("Connection Menu")]
    [SerializeField] private InputField ipInput;
    [SerializeField] private InputField portInput;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button connectionMenuBackButton;
    public InputField IpInput => ipInput;
    public InputField PortInput => portInput;
    #endregion
    #region Upgrades Menu
    [Header("Upgrades Menu")]
    [SerializeField] private Button upgradesMenuBackButton;
    [SerializeField] private WeaponUpgraderUI weaponUpgraderUI;
    #endregion
    #region Actions
    [Header("Actions")]
    public Action OnStartOfflineMode;
    public Action OnStartOnlinePVPMode;
    public Action OnStartOnlineCoopMode;
    public Action OnStartHost;
    public Action OnStartClient;
    public Action OnShowUpgrades;
    #endregion
    #region Public Properties
    public Button UpgradesMenuBackButton => upgradesMenuBackButton;
    #endregion

    #region Functions
    private void OnEnable()
    {
        AddUpgradesMenuButtonEvents();
    }

    private void OnDisable()
    {
        RemoveUpgradesMenuButtonEvents();
    }
    private void Start()
    {
        ShowMainMenu();
        AddMainMenuButtonEvents();
        AddPlayMenuButtonEvents();
        AddMultiplayerModesMenuButtonEvents();
        AddConnectionMenuButtonEvents();
        
    }

    void AddMainMenuButtonEvents()
    {
        playButton.onClick.AddListener(ShowPlayMenu);
        upgradesButton.onClick.AddListener(ShowUpgradesMenu);
        exitButton.onClick.AddListener(ExitGame);
    }

    void AddPlayMenuButtonEvents()
    {
        onlineButton.onClick.AddListener(ShowMultiplayerModesMenu);
        offlineButton.onClick.AddListener(StartOfflineMode);
        playMenuBackButton.onClick.AddListener(ShowMainMenu);
    }

    void AddMultiplayerModesMenuButtonEvents()
    {
        pvpModeButton.onClick.AddListener(OnPVPModeButtonPressed); 
        coopModeButton.onClick.AddListener(OnCoopModeButtonPressed);
        multiplayerModesMenuBackButton.onClick.AddListener(ShowPlayMenu);
    }

    void AddConnectionMenuButtonEvents()
    {

        hostButton.onClick.AddListener(OnHostButtonPressed);
        clientButton.onClick.AddListener(OnClientButtonPressed);
        connectionMenuBackButton.onClick.AddListener(ShowMultiplayerModesMenu);
    }

    void ShowMainMenu()
    {
        HideMenus();
        mainMenu.SetActive(true);
        weaponUpgraderUI.InitializeWeaponUI(weaponUpgraderUI.CurrentWeapon);
    }

    void ShowPlayMenu()
    {
        HideMenus();
        playMenu.SetActive(true);
    }

    void ShowMultiplayerModesMenu()
    {
        HideMenus();
        multiplayerModesMenu.SetActive(true);
    }

    void ShowUpgradesMenu()
    {
        OnShowUpgrades?.Invoke();
        HideMenus();
        upgradesMenu.SetActive(true);
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void StartOfflineMode()
    {
        OnStartOfflineMode?.Invoke();
    }

    void HideMenus()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(false);
        multiplayerModesMenu.SetActive(false);
        connectionMenu.SetActive(false);
    }

    void OnPVPModeButtonPressed()
    {
        HideMenus();
        connectionMenu.SetActive(true);
        OnStartOnlinePVPMode?.Invoke();
    }

    void OnCoopModeButtonPressed()
    {
        HideMenus();
        connectionMenu.SetActive(true);
        OnStartOnlineCoopMode?.Invoke();
    }

    void OnHostButtonPressed()
    {
        OnStartHost?.Invoke();
    }

    void OnClientButtonPressed()
    {
        OnStartClient?.Invoke();
    }

    void AddUpgradesMenuButtonEvents()
    {
        upgradesMenuBackButton.onClick.AddListener(ShowMainMenu);

    }

    void RemoveUpgradesMenuButtonEvents()
    {
        upgradesMenuBackButton.onClick.RemoveListener(ShowMainMenu);
    }
    #endregion
}