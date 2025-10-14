using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuActions : MonoBehaviour
{
    #region General components
    [Header("General components")]
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BoxManager boxManager;
    [SerializeField] private NetworkConnections networkConnections;
    #endregion
    [SerializeField] private MainMenuUI mainMenuUI;
    // Start is called before the first frame update
    void Awake()
    {
        mainMenuUI.OnStartOfflineMode += OnStartOfflineMode;
        mainMenuUI.OnStartLANHost += OnStartLANHost;
        mainMenuUI.OnStartLANClient += OnStartLANClient;
        mainMenuUI.OnStartCustomHost += OnStartCustomHost;
        mainMenuUI.OnStartCustomClient += OnStartCustomClient;
    }

    void OnStartOfflineMode()
    {
        sceneLoader.LoadOfflineScene(1);
    }

    void OnStartLANHost()
    {
        networkConnections.IPAddress = networkConnections.GetLanIp();
        networkConnections.StartHost();
        EnableBoxManager();
    }

    void EnableBoxManager()
    {
        if (!gameManager.ActiveBoxManager) return;
        boxManager.gameObject.SetActive(true);
        boxManager.Initialize();
    }

    void OnStartLANClient()
    {
        networkConnections.IPAddress = networkConnections.GetLanIp();
        networkConnections.StartClient();
    }

    void OnStartCustomHost()
    {
        networkConnections.IPAddress = mainMenuUI.IpInputField.text;
        networkConnections.StartHost();
        EnableBoxManager();
    }

    void OnStartCustomClient()
    {
        networkConnections.IPAddress = mainMenuUI.JoinIpInputField.text;
        networkConnections.StartClient();
    }
}
