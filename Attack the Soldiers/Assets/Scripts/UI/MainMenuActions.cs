using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    #region General components
    [Header("General components")]
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private NetworkGameManager networkGameManager;
    private UnityTransport transport;
    #endregion
    #region Functions
    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (mainMenuUI.PortInput != null)
            mainMenuUI.PortInput.text = "7777";
        mainMenuUI.IpInput.text = PlayerPrefs.GetString("last_ip", "192.168.");
    }

    void Awake()
    {
        mainMenuUI.OnStartOfflineMode += OnStartOfflineMode;
        mainMenuUI.OnStartOnlinePVPMode += SetPVPModeValues;
        mainMenuUI.OnStartOnlineCoopMode += SetCoopModeValues;
        mainMenuUI.OnStartHost += StartHost;
        mainMenuUI.OnStartClient += StartClient;
    }

    void OnStartOfflineMode()
    {
        networkGameManager.ActiveEnemyManager = false;
        networkGameManager.ActiveEnemyWaves = true;
        networkGameManager.StartOffline();
    }

    void SetPVPModeValues()
    {
        GameNetwork.MultiplayerModeType = MultiplayerModeType.PVP;
        networkGameManager.ActiveEnemyManager = false;
        networkGameManager.ActiveEnemyWaves = false;
    }

    void SetCoopModeValues()
    {
        GameNetwork.MultiplayerModeType = MultiplayerModeType.Coop;
        networkGameManager.ActiveEnemyManager = false;
        networkGameManager.ActiveEnemyWaves = true;
    }

    private void StartHost()
    {
        networkGameManager.StartOnline();
        ushort port = ushort.Parse(mainMenuUI.PortInput.text);
        transport.SetConnectionData("0.0.0.0", port);
        PlayerPrefs.SetString("last_ip", mainMenuUI.IpInput.text);
        PlayerPrefs.Save();
        NetworkManager.Singleton.StartHost();
    }

    private void StartClient()
    {
        networkGameManager.StartOnline();
        string ip = mainMenuUI.IpInput.text;
        ushort port = ushort.Parse(mainMenuUI.PortInput.text);
        transport.SetConnectionData(ip, port);
        PlayerPrefs.SetString("last_ip", ip);
        PlayerPrefs.Save();
        NetworkManager.Singleton.StartClient();
    }
    #endregion
}
