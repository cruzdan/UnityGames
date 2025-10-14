using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    #region General components
    [Header("General components")]
    //[SerializeField] private SceneLoader sceneLoader;
    //[SerializeField] private GameManager gameManager;
    //[SerializeField] private BoxManager boxManager;
    //[SerializeField] private NetworkConnections networkConnections;
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private ConnectionManager connectionManager;
    [SerializeField] private Unity.Netcode.Transports.UTP.UnityTransport unityTransport;
    #endregion
    void Awake()
    {
        mainMenuUI.OnStartOfflineMode += OnStartOfflineMode;
        mainMenuUI.OnStartLANHost += OnStartLANHost;
        mainMenuUI.OnStartLANClient += OnStartLANClient;
        mainMenuUI.OnStartCustomHost += OnStartCustomHost;
        mainMenuUI.OnStartCustomClient += OnStartCustomClient;
        mainMenuUI.OnSetLanIpPressed += OnSetLanIpPressed;
    }

    void OnStartOfflineMode()
    {
        //sceneLoader.LoadOfflineScene(1);
    }

    void OnStartLANHost()
    {
        //networkConnections.IPAddress = networkConnections.GetLanIp();
        ConnectFromMenuUI();
        EnableBoxManager();
    }

    void EnableBoxManager()
    {
        //if (!gameManager.ActiveBoxManager) return;
        //boxManager.gameObject.SetActive(true);
        //boxManager.Initialize();
    }

    void OnStartLANClient()
    {
        ConnectFromMenuUI();
        //networkConnections.IPAddress = networkConnections.GetLanIp();
        //networkConnections.StartClient();
    }

    void OnStartCustomHost()
    {
        //networkConnections.IPAddress = mainMenuUI.IpInputField.text;
        //networkConnections.StartHost();
        ConnectFromMenuUI();
        EnableBoxManager();
    }

    void OnStartCustomClient()
    {
        ConnectFromMenuUI();
        //networkConnections.IPAddress = mainMenuUI.JoinIpInputField.text;
        //networkConnections.StartClient();
    }

    void ConnectFromMenuUI()
    {
        //unityTransport.ConnectionData.Address = mainMenuUI.IpInputField.text;
        connectionManager.ProfileName = mainMenuUI.PlayerNameTextUI.text;
        connectionManager.SessionName = mainMenuUI.SessionIDUI.text;
        _ = connectionManager.CreateOrJoinSessionAsync();
    }

    void OnSetLanIpPressed()
    {
        mainMenuUI.IpInputField.text = connectionManager.GetLanIp();
    }
}
