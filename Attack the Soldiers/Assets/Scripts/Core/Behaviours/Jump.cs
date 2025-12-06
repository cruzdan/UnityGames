using Unity.Netcode;
using UnityEngine;

public class Jump : NetworkBehaviour
{
    #region Serialized Variables
    [Header("General")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 18f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float terminalVelocity = -15f;
    [SerializeField] private float minFall = -1.5f;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundRadius = 0.2f;
    #endregion
    #region Input
    [SerializeField] private PlayerInputSource playerInputSource;
    private IPlayerInputSource inputSource;
    #endregion
    #region Private Variables
    private float vertSpeed;
    private bool hitGround;
    private bool isTryingToJump;
    public bool isJumping;
    #endregion
    #region Public Properties
    public bool IsJumping => isJumping;
    public float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
    #endregion
    #region Functions
    void Start()
    {
        if (playerInputSource != null)
            inputSource = playerInputSource.GetInputSource();
    }

    public void HandleJump(bool isUsingKey)
    {
        hitGround = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundMask);
        isTryingToJump = !isUsingKey || inputSource.GetJump();

        if (isTryingToJump && hitGround)
            StartJump();
        if (hitGround)
            StopJump();
        else
            Falling();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertSpeed);
    }

    public bool IshittingGround()
    {
        return Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundMask); ;
    }

    void StartJump()
    {
        vertSpeed = jumpSpeed;
        hitGround = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertSpeed);
    }

    void StopJump()
    {
        vertSpeed = minFall;
        isJumping = false;
    }

    void Falling()
    {
        vertSpeed += gravity * 5 * Time.deltaTime;
        if (vertSpeed < terminalVelocity)
            vertSpeed = terminalVelocity;
        isJumping = true;
    }
    #endregion
}
