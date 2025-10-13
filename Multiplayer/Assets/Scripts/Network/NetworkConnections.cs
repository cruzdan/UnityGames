using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class NetworkConnections : MonoBehaviour
{
    #region General components
    [Header("General components")]
    [SerializeField] private UnityTransport unityTransport;
    #endregion
    #region IP 
    private string ipAddress;
    public string IPAddress
    {
        get { return ipAddress; }
        set { ipAddress = value; }
    }
    #endregion
    public void StartHost()
    {
        unityTransport.ConnectionData.Address = ipAddress;
        if (NetworkManager.Singleton.StartHost())
        {
            //Logger.Instance.LogInfo("Host started");
            NetworkObjectPool.Singleton.InitializePools();
            //EnableBoxManager();
            //HideNetworkButtons();
        }
        else
        {
            //Logger.Instance.LogError("Host could not be started");
        }
    }

    public void StartServer()
    {
        unityTransport.ConnectionData.Address = ipAddress;
        if (NetworkManager.Singleton.StartServer())
        {
            //Logger.Instance.LogInfo("Server started");
            NetworkObjectPool.Singleton.InitializePools();
            //EnableBoxManager();
            //HideNetworkButtons();
        }
        else
        {
            //Logger.Instance.LogError("Server could not be started");
        }
    }

    public void StartClient()
    {
        unityTransport.ConnectionData.Address = ipAddress;
        if (NetworkManager.Singleton.StartClient())
        {
            //Logger.Instance.LogInfo("Client started");
            //HideNetworkButtons();
        }
        else
        {
            //Logger.Instance.LogError("Client could not be started");
        }
    }

    public string GetLanIp()
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
                        return addr.Address.ToString();                        
                    }
                }
            }
        }
        // If not found, assigns localhost as fallback
        ipAddress = "127.0.0.1";
        return ipAddress;
    }
}
