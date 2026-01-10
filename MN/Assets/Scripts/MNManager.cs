using UnityEngine;
using UnityEngine.UI;

public class MNManager : MonoBehaviour
{
    #region Serialized Variables
    [Header("General")]
    [SerializeField] private GameObject player;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Transform cam;
    [SerializeField] private PlayerMovement playerMovement;
    #endregion
    #region Public Variables
    public bool isRestarting = false;
    #endregion
    #region DeviceType
    [Header("Device Type")]
    [SerializeField] private bool overrideDeviceType = false;
    [SerializeField] private bool isAndroid = false;
    [SerializeField] private DeviceChecker deviceChecker;
    public bool OverrideDeviceType { get => overrideDeviceType; set => overrideDeviceType = value; }
    public bool IsAndroid { get => isAndroid; set => isAndroid = value; }
    #endregion
    #region UI
    [Header("UI Elements")]
    [SerializeField] private Toggle isOnMobileToggle;
    [SerializeField] private GameObject mobileCanvas;
    [SerializeField] private GameObject initialCanvas;
    [SerializeField] private Button playButton;
    #endregion

    #region Functions
    private void Awake()
    {
        playButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        isAndroid = isOnMobileToggle.isOn;
        if (!overrideDeviceType)
            mobileCanvas.SetActive(deviceChecker.IsOnMobile());
        else
            mobileCanvas.SetActive(IsAndroid);
        initialCanvas.SetActive(false);
        playerMovement.enabled = true;
    }

    public void Restart()
    {
        player.GetComponent<PlayerMovement>().Restart();
        player.transform.localEulerAngles = new Vector3(0, 0, 0);
        cam.position = new Vector3(0, 3.85f, -3f);
        DeleteObjectsWithTag("Enemy");
        DeleteObjectsWithTag("Wall");
        DeleteObjectsWithTag("Floor");
        DeleteObjectsWithTag("Spawner");
        DeleteObjectsWithTag("Trap");
        mapManager.GenerateMapLevel();
    }

    void DeleteObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }
    #endregion
}
