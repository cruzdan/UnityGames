using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region Actions
    [Header("Actions")]
    public Action OnStartLANHost;
    public Action OnStartLANClient;
    public Action OnStartCustomClient;
    public Action OnStartCustomHost;
    public Action OnStartOfflineMode;
    public Action OnSetLanIpPressed;
    #endregion
    #region Menus
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject serversMenu;
    [SerializeField] private GameObject connectMenu;
    private MainMenuState currentMainMenuState = MainMenuState.Main;
    private ConnectState currentConnectState;
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
    #region Servers Menu
    [Header("Servers Menu")]
    [SerializeField] private Button createServerButton;
    [SerializeField] private Button joinServerButton;
    [SerializeField] private Button createLANButton;
    [SerializeField] private Button joinLANButton;
    [SerializeField] private Button serversMenuBackButton;
    #endregion
    #region Connect Menu
    [Header("Connect Menu")]
    [SerializeField] private InputField ipInputField;
    [SerializeField] private InputField playerNameTextUI;
    [SerializeField] private InputField sessionIDUI;
    [SerializeField] private Button setLanIPButton;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button connectMenuBackButton;
    public InputField IpInputField => ipInputField;
    public InputField PlayerNameTextUI => playerNameTextUI;
    public InputField SessionIDUI => sessionIDUI;
    #endregion
    #region Functions
    private void Start()
    {
        ShowMainMenu();
        AddMainMenuButtonEvents();
        AddPlayMenuButtonEvents();
        AddServersMenuButtonEvents();
        AddConnectMenuButtonEvents();
    }

    void AddMainMenuButtonEvents()
    {
        playButton.onClick.AddListener(ShowPlayMenu);
        exitButton.onClick.AddListener(ExitGame);
    }

    void AddPlayMenuButtonEvents()
    {
        onlineButton.onClick.AddListener(ShowServersMenu);
        offlineButton.onClick.AddListener(StartOfflineMode);
        playMenuBackButton.onClick.AddListener(ShowMainMenu);
    }

    void AddServersMenuButtonEvents()
    {
        createServerButton.onClick.AddListener(() => { ShowConnectMenu(true, ConnectState.CustomCreate); }); 
        joinServerButton.onClick.AddListener(() => { ShowConnectMenu(true, ConnectState.CustomJoin); });
        createLANButton.onClick.AddListener(() => { ShowConnectMenu(false, ConnectState.LanCreate); });
        joinLANButton.onClick.AddListener(() => { ShowConnectMenu(false, ConnectState.LanJoin); });
        serversMenuBackButton.onClick.AddListener(ShowPlayMenu);
    }

    void AddConnectMenuButtonEvents()
    {
        setLanIPButton.onClick.AddListener(LanIpPressed);
        connectButton.onClick.AddListener(Connect);
        connectMenuBackButton.onClick.AddListener(ShowServersMenu);
    }

    void Connect()
    {
        switch (currentConnectState)
        {
            case ConnectState.CustomCreate:
                StartCustomHost();
                break;
            case ConnectState.CustomJoin:
                StartCustomClient();
                break;
            case ConnectState.LanCreate:
                StartLANHost();
                break;
            case ConnectState.LanJoin:
                StartLANClient();
                break;
        }
    }

    void ShowMainMenu()
    {
        currentMainMenuState = MainMenuState.Main;
        HideMenus();
        mainMenu.SetActive(true);
    }

    void ShowPlayMenu()
    {
        currentMainMenuState = MainMenuState.Play;
        HideMenus();
        playMenu.SetActive(true);
    }

    void ShowServersMenu()
    {
        currentMainMenuState = MainMenuState.Servers;
        HideMenus();
        serversMenu.SetActive(true);
    }

    void ShowConnectMenu(bool enableIPInputField, ConnectState connectState)
    {
        currentMainMenuState = MainMenuState.Connect;
        ipInputField.interactable = enableIPInputField;
        HideMenus();
        connectMenu.SetActive(true);
        currentConnectState = connectState;
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void StartOfflineMode()
    {
        Debug.Log("Starting offline mode...");
        OnStartOfflineMode?.Invoke();
    }

    void StartLANHost()
    {
        Debug.Log("Starting LAN Host...");
        OnStartLANHost?.Invoke();
        HideMenus();
    }

    void StartLANClient()
    {
        Debug.Log("Starting LAN Client...");
        OnStartLANClient?.Invoke();
        HideMenus();
    }

    void StartCustomHost()
    {
        Debug.Log("Starting Custom Host...");
        OnStartCustomHost?.Invoke();
        HideMenus();
    }

    void StartCustomClient()
    {
        Debug.Log("Starting Custom Client...");
        OnStartCustomClient?.Invoke();
        HideMenus();
    }

    void LanIpPressed()
    {
        Debug.Log("LAN IP Pressed...");
        OnSetLanIpPressed?.Invoke();
    }

    void HideMenus()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(false);
        serversMenu.SetActive(false);
        connectMenu.SetActive(false);
    }
    #endregion
}

enum MainMenuState
{
    Main = 0,
    Play = 1,
    Servers = 2,
    Connect = 3
}

enum ConnectState
{
    CustomCreate = 0,
    CustomJoin = 1,
    LanCreate = 2,
    LanJoin = 3
}