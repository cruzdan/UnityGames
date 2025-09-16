using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Image panel;
    [SerializeField] private GameObject boxManager;
    [SerializeField] private UnityTransport unityTransport;
    [SerializeField] private GameObject idText;
    private string ipAddress;
    public void SetIdFromTextField(string newIp)
    {
        ipAddress = newIp;
    }

    private void Awake()
    {
		Application.targetFrameRate = 60;
        hostButton.onClick.AddListener(() => {
            unityTransport.ConnectionData.Address = ipAddress;
            if (NetworkManager.Singleton.StartHost())
            {
                Logger.Instance.LogInfo("Host started");
                NetworkObjectPool.Singleton.InitializePools();
                boxManager.SetActive(true);
                HideNetworkButtons();
            }
            else
            {
                Logger.Instance.LogError("Host could not be started");
            }
        });
        serverButton.onClick.AddListener(() => {
            unityTransport.ConnectionData.Address = ipAddress;
            if (NetworkManager.Singleton.StartServer())
            {
                Logger.Instance.LogInfo("Server started");
                NetworkObjectPool.Singleton.InitializePools();
                boxManager.SetActive(true);
                HideNetworkButtons();
            }
            else
            {
                Logger.Instance.LogError("Server could not be started");
            }
        });
        clientButton.onClick.AddListener(() => {
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
        });
    }

    void HideNetworkButtons()
    {
        hostButton.gameObject.SetActive(false);
        serverButton.gameObject.SetActive(false);
        clientButton.gameObject.SetActive(false);
        idText.SetActive(false);
        Logger.Instance.gameObject.SetActive(false);
        Color a = panel.color;
        a.a = 0;
        panel.color = a;
    }
}
