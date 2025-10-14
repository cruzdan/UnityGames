using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region General components
    [Header("General components")]
    public Action OnStartLANHost;
    public Action OnStartLANClient;
    public Action OnStartCustomClient;
    public Action OnStartCustomHost;
    public Action OnStartOfflineMode;
    #endregion
    #region Menus
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject serversMenu;
    [SerializeField] private GameObject createCustomServerMenu;
    [SerializeField] private GameObject joinCustomServerMenu;
    private MainMenuState currentMainMenuState = MainMenuState.main;
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
    #region Create Custom Server Menu
    [Header("Create Custom Server Menu")]
    [SerializeField] private InputField ipInputField;
    [SerializeField] private Button createCustomServerStartButton;
    [SerializeField] private Button createCustomServerBackButton;
    public InputField IpInputField { get { return ipInputField; } }
    #endregion
    #region Join Custom Server Menu
    [Header("Join Custom Server Menu")]
    [SerializeField] private InputField joinIpInputField;
    [SerializeField] private Button setLANIpButton;
    [SerializeField] private Button joinCustomServerStartButton;
    [SerializeField] private Button joinCustomServerBackButton;
    public InputField JoinIpInputField { get { return joinIpInputField; } }
    #endregion
    #region Servers Menu
    [Header("Servers Menu")]
    [SerializeField] private Button createServerButton;
    [SerializeField] private Button joinServerButton;
    [SerializeField] private Button createLANButton;
    [SerializeField] private Button joinLANButton;
    [SerializeField] private Button serversMenuBackButton;
    #endregion
    #region Functions
    private void Start()
    {
        ShowMainMenu();
        AddMainMenuButtonEvents();
        AddPlayMenuButtonEvents();
        AddServersMenuButtonEvents();
        AddCreateServerMenuButtonEvents();
        AddJoinServerMenuButtonEvents();
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
        createLANButton.onClick.AddListener(StartLANHost);
        joinLANButton.onClick.AddListener(StartLANClient);
        serversMenuBackButton.onClick.AddListener(ShowPlayMenu);
        createServerButton.onClick.AddListener(ShowCreateCustomServerMenu);
        joinServerButton.onClick.AddListener(ShowJoinCustomServerMenu);
    }

    void AddCreateServerMenuButtonEvents()
    {
        //setLANIpButton.onClick.AddListener(() => ipInputField.text = networkConnections.GetLanIp());
        createCustomServerStartButton.onClick.AddListener(StartCustomHost);
        createCustomServerBackButton.onClick.AddListener(ShowServersMenu);
    }

    void AddJoinServerMenuButtonEvents()
    {
        joinCustomServerStartButton.onClick.AddListener(StartCustomClient);
        joinCustomServerBackButton.onClick.AddListener(ShowServersMenu);
    }

    void ShowMainMenu()
    {
        currentMainMenuState = MainMenuState.main;
        HideMenus();
        mainMenu.SetActive(true);
    }

    void ShowPlayMenu()
    {
        currentMainMenuState = MainMenuState.play;
        HideMenus();
        playMenu.SetActive(true);
    }

    void ShowServersMenu()
    {
        currentMainMenuState = MainMenuState.servers;
        HideMenus();
        serversMenu.SetActive(true);
    }

    void ShowCreateCustomServerMenu()
    {
        currentMainMenuState = MainMenuState.createCustomServer;
        HideMenus();
        createCustomServerMenu.SetActive(true);
    }

    void ShowJoinCustomServerMenu()
    {
        currentMainMenuState = MainMenuState.joinCustomServer;
        HideMenus();
        joinCustomServerMenu.SetActive(true);
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

    

    void HideMenus()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(false);
        serversMenu.SetActive(false);
        createCustomServerMenu.SetActive(false);
        joinCustomServerMenu.SetActive(false);
    }
    #endregion
}

public enum MainMenuState
{
    main = 0,
    play = 1,
    servers = 2,
    createCustomServer = 3,
    joinCustomServer = 4
}