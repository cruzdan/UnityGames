using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5FloorMountainBehaviour : MonoBehaviour
{
    //movementSpeedX need to be > 0
    [SerializeField] private float movementSpeedX;
    //movementSpeedY need to be > 0
    [SerializeField] private float movementSpeedY;
    //movementSpeedY need to be < 0
    [SerializeField] private float fallSpeedY;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsFloor;
    [SerializeField] private Transform checkTransform;
    [SerializeField] private ShootToShip shootToShip;

    private float slopeDistanceX;
    private bool isGrounded;
    private bool isOnSlope;
    private bool paused = false;
    //paused speed need to be > 0
    [SerializeField] private float pausedSpeed;
    private int shoots = 0;
    [SerializeField] private BoundsPoolObject bounds;
    private Animator animator;

    [SerializeField] private GameObject ship;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();

        slopeDistanceX = cc.size.x / 2.0f;
        animator = GetComponent<Animator>();
    }

    public void SetSpeedX(float speed) { movementSpeedX = speed; }
    public void SetSpeedY(float speed) { movementSpeedY = speed; }
    public void SetFallSpeed(float speed) { fallSpeedY = speed; }
    public void SetPausedSpeed(float speed) { pausedSpeed = speed; }
    public void SetShip(GameObject newShip) { ship = newShip; }
    public void SetPaused(bool value) { paused = value; }
    public void SetShoots(int value) { shoots = value; }
    private void FixedUpdate()
    {
        if (!paused)
        {
            CheckGround();
            CheckSlope();
            ApplyMovement();

            if (transform.position.x >= 0f)
            {
                paused = true;
                animator.SetBool("Stop", true);
                shootToShip.enabled = true;
            }
        }
        else
        {
            ChangeAimToShip();
            MoveInPause();
            if (shootToShip.shoots > shoots)
            {
                paused = false;
                animator.SetBool("Stop", false);
                shoots = shootToShip.shoots;
                shootToShip.enabled = false;
                transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                if (shoots > 2)
                {
                    movementSpeedX = -(movementSpeedX + movementSpeedX);
                    bounds.enabled = true;
                    transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                }
            }
        }
    }

    void ChangeAimToShip()
    {
        if (ship.transform.position.x < transform.position.x)
        {
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    private void MoveInPause()
    {
        rb.velocity = new Vector2(-pausedSpeed, 0f);
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(checkTransform.position, Vector2.down, 0.06f, whatIsFloor);
        if (hit)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void CheckSlope()
    {
        RaycastHit2D slopeHitFront;
        if (movementSpeedX > 0)
        {
            slopeHitFront = Physics2D.Raycast(checkTransform.position, Vector2.right, slopeDistanceX,
            whatIsGround);
        }
        else
        {
            slopeHitFront = Physics2D.Raycast(checkTransform.position, -Vector2.right, slopeDistanceX,
            whatIsGround);
        }


        if (slopeHitFront)
        {
            isOnSlope = true;
        }
        else
        {
            isOnSlope = false;
        }
    }

    private void ApplyMovement()
    {
        if (isGrounded)
        {
            //is on the floor
            rb.velocity = new Vector2(movementSpeedX, 0.0f);
        }
        else if (!isOnSlope)
        {
            //is on the air
            rb.velocity = new Vector2(movementSpeedX, fallSpeedY);
        }
        if (isOnSlope)
        {
            //is on a slope
            rb.velocity = new Vector2(movementSpeedX, movementSpeedY);
        }
    }
}