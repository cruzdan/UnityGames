using Unity.Netcode;
using UnityEngine;
//Class in charge of general enemy movement
public class EnemyMovement : MonoBehaviour
{
    #region General
    [Header("General")]
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Jump jump;

    #endregion
    #region Movement
    [Header("Movement")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform missingGroundCheck;
    [SerializeField] protected float checkRadius = 0.2f;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float speed = 3;
    [SerializeField] protected float initialSpeed = 3;
    protected bool facingRight = true;
    protected Vector2 movement;
    #endregion
    #region Auxiliar Variables
    protected float direction;
    protected float distance;
    protected bool hitGround;
    protected bool isGroundInFront;
    protected bool isWallInFront;
    private Vector2 rightFacingAngle = Vector2.zero;
    private Vector2 leftFacingAngle = new(0, 180);
    #endregion
    #region Public Properties
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public bool IsFacingRight { get => facingRight; set => facingRight = value; } 
    public float Speed { get => speed; set => speed = value; }
    public float InitialSpeed { get => initialSpeed; set => initialSpeed = value; }
    #endregion
    #region Functions
    public virtual void Chase()
    {
        FollowTarget();
        FlipEnemyDirectionIfPossible();
        JumpIfPossible();
        PassToAttackIfPossible();
    }

    public virtual void StartChase()
    {
    }

    protected void FollowTarget()
    {
        movement = new Vector2(Mathf.Sign(direction) * speed, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    public void FlipEnemyDirectionIfPossible()
    {
        direction = enemy.PlayerTarget.transform.position.x - transform.position.x;
        if (direction > 0 && !facingRight)
            Flip();
        else if (direction < 0 && facingRight)
            Flip();
    }

    protected void Flip()
    {
        facingRight = !facingRight;
        transform.localEulerAngles = facingRight ? rightFacingAngle : leftFacingAngle;
    }

    protected void JumpIfPossible()
    {
        isWallInFront = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);
        isGroundInFront = Physics2D.OverlapCircle(missingGroundCheck.position, checkRadius, groundLayer);
        if (!isGroundInFront || isWallInFront)
        {
            jump.HandleJump(false);
        }
    }

    protected void PassToAttackIfPossible()
    {
        hitGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        distance = Vector2.Distance(transform.position, enemy.PlayerTarget.transform.position);
        if (Mathf.Abs(distance) <= enemy.EnemyAttack.AttackRange && hitGround)
        {
            enemy.StartAttack();
            movement = Vector2.zero;
            movement.y = rb.linearVelocity.y;
            rb.linearVelocity = movement;
        }
    }

    public bool IsGroundInFront()
    {
        return Physics2D.OverlapCircle(missingGroundCheck.position, checkRadius, groundLayer);
    }
    #endregion
}
