using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Image panel;
    [SerializeField] private GameObject boxManager;

    private void Awake()
    {
		Application.targetFrameRate = 60;
        hostButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartHost())
            {
                Logger.Instance.LogInfo("Host started");
                NetworkObjectPool.Singleton.InitializePools();
                boxManager.SetActive(true);
                HideNetworkButtons();
            }
            else
            {
                Logger.Instance.LogInfo("Host could not be started");
            }
        });
        serverButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartServer())
            {
                Logger.Instance.LogInfo("Server started");
                NetworkObjectPool.Singleton.InitializePools();
                boxManager.SetActive(true);
                HideNetworkButtons();
            }
            else
            {
                Logger.Instance.LogInfo("Server could not be started");
            }
        });
        clientButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartClient())
            {
                Logger.Instance.LogInfo("Client started");
                HideNetworkButtons();
            }
            else
            {
                Logger.Instance.LogInfo("Client could not be started");
            }
        });
    }

    void HideNetworkButtons()
    {
        hostButton.gameObject.SetActive(false);
        serverButton.gameObject.SetActive(false);
        clientButton.gameObject.SetActive(false);
        Color a = panel.color;
        a.a = 0;
        panel.color = a;
    }
}
