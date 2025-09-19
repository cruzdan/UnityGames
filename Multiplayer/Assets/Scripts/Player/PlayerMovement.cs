using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("General")]
    [SerializeField] private bool isOffline = false; // ? Modo offline

    [Header("Walk")]
    [SerializeField] private float walkSpeedX = 5.0f;
    private float currentSpeed = 0;
    private Rigidbody2D rb;
    private Vector2 movement;

    [Header("Run")]
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float multiplier = 1;
    [SerializeField] private float multiplierTime = 5;

    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    [SerializeField] private float regStamAmount = 4f;
    [SerializeField] private float redStamAmount = 2;
    [SerializeField] private float RegStamTime = 0.2f;
    [SerializeField] private float reduceStaminaTime = 0.2f;
    [SerializeField] private float timeToStartRegenerateStamina = 3f;

    private float timerRegStamina;
    private float timerReduceStamina;
    private float timerRegenerateStamina;
    private float timerMultiplier;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float terminalVelocity = -15f;
    [SerializeField] private float minFall = -1.5f;
    [SerializeField] private float vertSpeed;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundRadius = 0.2f;
    private bool hitGround;

    [SerializeField] private PlayerUI playerUI;

    private Animator animator;
    private PlayerInput playerInput;

    void Start()
    {
        if (!isOffline && !IsOwner) return;

        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
        timerRegStamina = timeToStartRegenerateStamina;
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!CanControl()) return;

        HandlePause();
        HandleRun();
        HandleStaminaRegen();
    }

    private void FixedUpdate()
    {
        if (!CanControl()) return;

        HandleMovement();
        HandleJump();
    }

    // ? Decidir si este jugador puede mover
    private bool CanControl()
    {
        return isOffline || IsOwner;
    }

    private void HandlePause()
    {
        if (playerInput.actions["Pause"].WasPressedThisFrame())
        {
            playerUI.ChangePauseMenu();
        }
    }

    private void HandleRun()
    {
        if (playerInput.actions["Run"].IsPressed() && currentStamina > 0)
        {
            currentSpeed = runSpeed;
            if (timerReduceStamina <= 0f)
            {
                currentStamina -= redStamAmount;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                playerUI.SetStaminaText(currentStamina.ToString());
                playerUI.SetStaminaWidth(currentStamina * 0.01f);
                timerReduceStamina = reduceStaminaTime;
            }
            else
            {
                timerReduceStamina -= Time.deltaTime;
            }

            timerRegStamina = timeToStartRegenerateStamina;
        }
        else
        {
            currentSpeed = walkSpeedX;
        }
    }

    private void HandleStaminaRegen()
    {
        if (currentStamina < maxStamina)
        {
            if (timerRegStamina > 0f)
            {
                timerRegStamina -= Time.deltaTime;
            }
            else
            {
                if (timerRegenerateStamina <= 0f)
                {
                    timerRegenerateStamina = RegStamTime;
                    currentStamina += regStamAmount;
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                    playerUI.SetStaminaText(currentStamina.ToString());
                    playerUI.SetStaminaWidth(currentStamina * 0.01f);
                }
                else
                {
                    timerRegenerateStamina -= Time.deltaTime;
                }
            }
        }

        if (timerMultiplier > 0)
        {
            timerMultiplier -= Time.deltaTime;
            if (timerMultiplier <= 0)
            {
                multiplier = 1;
            }
        }
    }

    private void HandleMovement()
    {
        movement = Vector2.zero;

        if (playerInput.actions["Right"].IsPressed())
        {
            if (transform.localEulerAngles.y != 0)
            {
                if (!isOffline) 
                    ChangeAnglesServerRpc(0, 0);
                else
                    transform.localEulerAngles = new Vector2(0, 0);
            }
            movement.x = currentSpeed * multiplier;
        }

        if (playerInput.actions["Left"].IsPressed())
        {
            if (transform.localEulerAngles.y != 180)
            {
                if (!isOffline) 
                    ChangeAnglesServerRpc(0, 180);
                else
                transform.localEulerAngles = new Vector2(0, 180);
            }
            movement.x = -currentSpeed * multiplier;
        }
        rb.velocity = new Vector2(movement.x, rb.velocity.y);
    }

    private void HandleJump()
    {
        hitGround = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundMask);

        if (playerInput.actions["Jump"].IsPressed() && hitGround)
        {
            vertSpeed = jumpSpeed;
            hitGround = false;
        }

        animator.SetBool("OnGround", hitGround);

        if (hitGround)
        {
            vertSpeed = minFall;
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
                vertSpeed = terminalVelocity;
        }

        rb.velocity = new Vector2(rb.velocity.x, vertSpeed);
        animator.SetFloat("Horizontal", Mathf.Abs(movement.x));
    }

    [ServerRpc]
    void ChangeAnglesServerRpc(float x, float y)
    {
        transform.localEulerAngles = new Vector2(x, y);
    }

    public void SetMaxStamina()
    {
        currentStamina = maxStamina;
        timerReduceStamina = 0;
        timerRegenerateStamina = 0;
        playerUI.SetStaminaText(currentStamina.ToString());
        playerUI.SetStaminaWidth(currentStamina * 0.01f);
    }

    [ClientRpc]
    public void SetMultiplierClientRpc(float value, ClientRpcParams clientRpcParams = default)
    {
        SetMultiplier(value);
    }

    public void SetMultiplier(float value)
    {
        multiplier = value;
        timerMultiplier = multiplierTime;
    }

    public void RestartVelocity()
    {
        rb.velocity = Vector2.zero;
        animator.SetFloat("Horizontal", 0);
        animator.SetBool("OnGround", true);
    }
}
