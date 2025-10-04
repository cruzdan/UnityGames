using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

//Class in charge of the application start menu
public class NetworkManagerUI : MonoBehaviour
{
    #region Buttons
    [Header("Buttons")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button lanButton;
    [SerializeField] private Button offlineButton;
    #endregion
    #region Panel
    [Header("Panel")]
    [SerializeField] private Image panel;
    #endregion
    #region General components
    [Header("General components")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject boxManager;
    [SerializeField] private UnityTransport unityTransport;
    #endregion
    #region IP 
    [Header("IP")]
    [SerializeField] private GameObject idText;
    private string ipAddress;
    #endregion
    #region Functions
    private void Awake()
    {
        SetFPS(60);
        AddHostButtonEvents();
        AddServerButtonEvents();
        AddClientButtonEvents();
        AddLanButtonEvents();
        AddOfflineButtonEvents();
    }

    void AddHostButtonEvents()
    {
        hostButton.onClick.AddListener(StartHost);
    }

    void StartHost()
    {
        unityTransport.ConnectionData.Address = ipAddress;
        if (NetworkManager.Singleton.StartHost())
        {
            Logger.Instance.LogInfo("Host started");
            NetworkObjectPool.Singleton.InitializePools();
            EnableBoxManager();
            HideNetworkButtons();
        }
        else
        {
            Logger.Instance.LogError("Host could not be started");
        }
    }

    void AddServerButtonEvents()
    {
        serverButton.onClick.AddListener(StartServer);
    }

    void StartServer()
    {
        unityTransport.ConnectionData.Address = ipAddress;
        if (NetworkManager.Singleton.StartServer())
        {
            Logger.Instance.LogInfo("Server started");
            NetworkObjectPool.Singleton.InitializePools();
            EnableBoxManager();
            HideNetworkButtons();
        }
        else
        {
            Logger.Instance.LogError("Server could not be started");
        }
    }

    void AddClientButtonEvents()
    {
        clientButton.onClick.AddListener(StartClient);
    }

    void StartClient()
    {
        unityTransport.ConnectionData.Address = ipAddress;
        if (NetworkManager.Singleton.StartClient())
        {
            Logger.Instance.LogInfo("Client started");
            HideNetworkButtons();
        }
        else
        {
            Logger.Instance.LogError("Client could not be started");
        }
    }

    void AddLanButtonEvents()
    {
        lanButton.onClick.AddListener(SetLanIp);
    }

    void SetLanIp()
    {
        // Gets the first valid IPv4 address that is not loopback or virtual
        foreach (var ni in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up &&
                ni.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Loopback)
            {
                var ipProps = ni.GetIPProperties();
                foreach (var addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = addr.Address.ToString();
                        idText.GetComponent<InputField>().text = ipAddress;
                        return;
                    }
                }
            }
        }
        // If not found, assigns localhost as fallback
        ipAddress = "127.0.0.1";
        idText.GetComponent<InputField>().text = ipAddress;
    }

    void AddOfflineButtonEvents()
    {
        offlineButton.onClick.AddListener(StartOfflineMode);
    }

    void StartOfflineMode()
    {
        EnableBoxManager();
        HideNetworkButtons();
    }

    void HideNetworkButtons()
    {
        hostButton.gameObject.SetActive(false);
        serverButton.gameObject.SetActive(false);
        clientButton.gameObject.SetActive(false);
        idText.SetActive(false);
        Logger.Instance.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
    }

    public void SetIdFromTextField(string newIp)
    {
        ipAddress = newIp;
    }

    void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }

    void EnableBoxManager()
    {
        if (!gameManager.ActiveBoxManager) return;
        boxManager.SetActive(true);
        boxManager.GetComponent<BoxManager>().Initialize();
    }
    #endregion
}
