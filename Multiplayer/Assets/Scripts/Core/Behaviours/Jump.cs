using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Input")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 18f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float terminalVelocity = -15f;
    [SerializeField] private float minFall = -1.5f;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundRadius = 0.2f;
    private float vertSpeed;
    private bool hitGround;
    private bool isTryingToJump;

    public void HandleJump(bool isUsingKey)
    {
        hitGround = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundMask);

        isTryingToJump = isUsingKey ? Input.GetKey(jumpKey) : true;

        if (isTryingToJump && hitGround)
        {
            vertSpeed = jumpSpeed;
            hitGround = false;
            rb.velocity = new Vector2(rb.velocity.x, vertSpeed);
        }
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
    }
}
