using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private bool isOffline = false;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Shoot shoot;
    private float shootTimer = 0;
    #endregion
    #region Functions
    private void Start()
    {
        if (!isOffline && !IsOwner) return;
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!isOffline && !IsOwner) return;
        if (playerInput == null) { enabled = false; return; }
        ;
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
            return;
        }
        if (playerInput.actions["Shoot"].IsPressed())
        {
            shoot.ShootCurrentWeapon();
            playerUI.SetBulletText(shoot.CurrentBullets.ToString());
            shootTimer = shoot.CurrentBulletInfo.TimeToShoot;
        }
    }
    #endregion
}
