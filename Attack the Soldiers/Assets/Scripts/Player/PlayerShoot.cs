using Unity.Netcode;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
    #region Serialized Variables
    [Header("Serialized Variables")]
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Shoot shoot;
    [SerializeField] private HoldButton shootHoldButton;
    #endregion
    #region Input
    [SerializeField] private PlayerInputSource playerInputSource;
    private IPlayerInputSource inputSource;
    #endregion
    #region Private Variables
    private float shootTimer = 0;
    private bool canShoot = true;
    #endregion
    #region Public Properties
    public bool CanShoot { get { return canShoot; } set { canShoot = value; } }
    #endregion
    #region Functions
    void Awake()
    {
        inputSource = playerInputSource.GetInputSource();
    }

    private void Start()
    {
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) enabled = false;
        if (playerInputSource == null) enabled = false;
    }

    void Update()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
            return;
        }
        if (!canShoot) return;
        if (inputSource.GetShoot())
        {
            shoot.ShootCurrentWeapon();
            shootTimer = shoot.CurrentBulletInfo.TimeToShoot;
        }
    }
    #endregion
}
