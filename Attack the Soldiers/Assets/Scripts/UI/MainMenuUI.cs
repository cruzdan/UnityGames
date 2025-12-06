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
    #endregion
    #region Main Menu
    [Header("Main Menu")]
    [SerializeField] private Button playButton;
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
    #region Actions
    [Header("Actions")]
    public Action OnStartOfflineMode;
    public Action OnStartOnlinePVPMode;
    public Action OnStartOnlineCoopMode;
    public Action OnStartHost;
    public Action OnStartClient;
    #endregion
    #region Functions
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

    void ExitGame()
    {
        Application.Quit();
    }

    void StartOfflineMode()
    {
        //UICanvas.SetActive(false);
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
        //UICanvas.SetActive(false);
    }

    void OnClientButtonPressed()
    {
        OnStartClient?.Invoke();
        //UICanvas.SetActive(false);
    }
    #endregion
}