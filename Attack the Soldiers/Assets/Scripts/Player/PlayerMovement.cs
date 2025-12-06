using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private Stamina playerStamina;
    [SerializeField] private Jump jump;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerAnimationController anim;
    #endregion
    #region Input
    [Header("Input")]
    [SerializeField] private IPlayerInputSource inputSource;
    [SerializeField] private PlayerInputSource playerInputSource;
    #endregion
    #region Walk Variables
    [Header("Walk")]
    [SerializeField] private float walkSpeedX = 5.0f;
    private float currentSpeed = 0;
    private Vector2 movement;
    #endregion
    #region Run Variables
    [Header("Run")]
    [SerializeField] private float runSpeed = 8f;
    private bool isRunning = false;
    private bool useStamina = true;
    #endregion
    #region Speed
    [Header("Speed Multiplier")]
    [SerializeField] private float speedMultiplier = 1;
    [SerializeField] private float speedMultiplierTime = 5;
    private float speedTimerMultiplier;
    #endregion
    #region Public Properties
    public float WalkSpeedX { get { return walkSpeedX; } }
    public bool UseStamina { get { return useStamina; } set { useStamina = value; } }
    public Jump Jump { get { return jump; } }
    #endregion
    #region Functions
    void Awake()
    {
        inputSource = playerInputSource.GetInputSource();
    }

    private void Start()
    {
        if (!GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) enabled = false;
    }

    void Update()
    {
        HandleRun();
        HandleSpeedMultiplierTime();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        jump.HandleJump(true);
    }

    private void HandleRun()
    {
        bool wantsToRun = inputSource.GetRun();
        if (playerStamina != null && useStamina)
            HandleRunWithStamina(wantsToRun);
        else
            HandleRunWithoutStamina(wantsToRun);
    }

    void HandleRunWithStamina(bool wantsToRun)
    {
        if (wantsToRun && playerStamina.CurrentStamina > 0)
        {
            currentSpeed = runSpeed;
            playerStamina.UpdateStamina();
            isRunning = true;
        }
        else
        {
            currentSpeed = walkSpeedX;
            isRunning = false;
        }
    }

    void HandleRunWithoutStamina(bool wantsToRun)
    {
        currentSpeed = wantsToRun ? runSpeed : walkSpeedX;
        isRunning = wantsToRun;
    }

    void HandleSpeedMultiplierTime()
    {
        if (speedTimerMultiplier > 0)
        {
            speedTimerMultiplier -= Time.deltaTime;
            if (speedTimerMultiplier <= 0)
            {
                speedMultiplier = 1;
            }
        }
    }

    void HandleAnimation()
    {
        if (jump.IsJumping)
            anim.SetJump();
        else if (isRunning)
            anim.SetRun();
        else if (Mathf.Abs(movement.x) > 0.1f)
            anim.SetWalk();
        else
            anim.SetIdle();
    }

    private void HandleMovement()
    {
        movement = Vector2.zero;
        float horizontalMovement = inputSource.GetHorizontalMovement();
        switch (horizontalMovement)
        {
            case 1:
                if (transform.localEulerAngles.y != 0)
                {
                    if (GameNetwork.Instance.IsOnline)
                        ChangeAnglesServerRpc(0, 0);
                    else
                        ChangeAngles(0, 0);
                }
                movement.x = currentSpeed * speedMultiplier;
                break;
            case -1:
                if (transform.localEulerAngles.y != 180)
                {
                    if (GameNetwork.Instance.IsOnline)
                        ChangeAnglesServerRpc(0, 180);
                    else
                        ChangeAngles(0, 180);
                }
                movement.x = -currentSpeed * speedMultiplier;
                break;
        }
        rb.linearVelocity = new Vector2(movement.x, rb.linearVelocity.y);
    }

    [Rpc(SendTo.ClientsAndHost)]
    void ChangeAnglesClientRpc(float x, float y)
    {
        ChangeAngles(x, y);
    }

    [Rpc(SendTo.Server)]
    void ChangeAnglesServerRpc(float x, float y)
    {
        ChangeAnglesClientRpc(x, y);
    }

    void ChangeAngles(float x, float y)
    {
        transform.localEulerAngles = new Vector2(x, y);
    }

    [ClientRpc]
    public void SetSpeedMultiplierClientRpc(float value, ClientRpcParams clientRpcParams = default)
    {
        SetSpeedMultiplier(value);
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = value;
        speedTimerMultiplier = speedMultiplierTime;
    }

    [ClientRpc]
    public void SetWalkSpeedClientRpc(float value, ClientRpcParams clientRpcParams = default)
    {
        SetWalkSpeed(value);
    }

    public void SetWalkSpeed(float value)
    {
        walkSpeedX = value;
    }

    [ClientRpc]
    public void SetUseStaminaClientRpc(bool value, ClientRpcParams clientRpcParams = default)
    {
        SetUseStamina(value);
    }

    public void SetUseStamina(bool value)
    {
        useStamina = value;
    }

    public void RestartVelocity()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetIdle();
    }

    [ClientRpc]
    public void SetJumpForceClientRpc(float value, ClientRpcParams clientRpcParams = default)
    {
        SetJumpForce(value);
    }

    void SetJumpForce(float value)
    {
        jump.JumpSpeed = value;
    }
    #endregion
}
