using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGameManager : NetworkBehaviour
{
    #region Bullet
    [Header("Bullet")]
    [SerializeField] private BulletInfoSO bulletInfoSO;
    #endregion
    #region Box
    [Header("Box")]
    [SerializeField] private BoxInfoSO boxInfoSO;
    [SerializeField] private BoxManager boxManager;
    #endregion
    #region Enemy
    [Header("Enemy")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private WaveManager waveManager;
    #endregion
    #region Player
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private PlayerManager playerManager;
    #endregion
    #region Device
    [SerializeField] private DeviceChecker deviceChecker;
    #endregion
    #region Options
    [Header("Options")]
    private bool activeBoxManager = true;
    private bool activeEnemyManager = true;
    private bool activeEnemyWaves = false;
    [SerializeField] private bool overrideDeviceType = false;
    [SerializeField] private bool isAndroid = false;
    #endregion
    #region Public Properties
    public BoxInfoSO BoxInfoSO => boxInfoSO;
    public bool ActiveBoxManager { get => activeBoxManager; set => activeBoxManager = value; }
    public bool ActiveEnemyManager { get => activeEnemyManager; set => activeEnemyManager = value; }
    public bool ActiveEnemyWaves { get => activeEnemyWaves; set => activeEnemyWaves = value; }
    public bool OverrideDeviceType { get => overrideDeviceType; set => overrideDeviceType = value; }
    public bool IsAndroid { get => isAndroid; set => isAndroid = value; }
    public BulletInfoSO BulletInfoSO => bulletInfoSO;
    #endregion
    #region UI
    [SerializeField] private Toggle isOnMobileToggle;
    [SerializeField] private GameObject mobileCanvas;
    #endregion
    #region Functions
    private void Awake()
    {
        boxInfoSO.InitializeBoxDictionary();
        bulletInfoSO.InitializeBulletInfoDictionary();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        ActiveOptions();
    }

    public void StartOffline()
    {
        GameNetwork.StartOffline();
        InitializeUI();
        NetworkObjectPool.Singleton.InitializeObjectPool();
        GameObject player = Instantiate(playerPrefab);
        player.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        bulletInfoSO.InitializeBulletInfoDictionary();
        playerManager.Players.Add(player.GetComponentInChildren<Player>());
        ActiveOptions();
    }

    public void InitializeUI()
    {
        isAndroid = isOnMobileToggle.isOn;
        //ActivateMobileMenu();
    }

    private void ActivateMobileMenu()
    {
        if (!overrideDeviceType)
        {
            mobileCanvas.SetActive(deviceChecker.IsOnMobile());
            Debug.Log("mobileCanvas active1: " + deviceChecker.IsOnMobile());
        }
        else
        {
            mobileCanvas.SetActive(isOnMobileToggle.isOn);
            Debug.Log("mobileCanvas active2: " + isOnMobileToggle.isOn);
        }
    }

    void ActiveOptions()
    {
        if (activeBoxManager)
            boxManager.Initialize();
        if (activeEnemyManager)
            enemyManager.StartSpawning();
        else if (activeEnemyWaves)
            waveManager.StartWaves();
    }

    public void StartOnline()
    {
        GameNetwork.StartOnline();
        InitializeUI();
    }
    #endregion
}
