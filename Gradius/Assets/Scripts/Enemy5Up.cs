using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5Up : MonoBehaviour
{
    //movementSpeedX need to be > 0
    [SerializeField] private float movementSpeedX;
    [SerializeField] private ShootToShip shootToShip;
    //pausedSpeed need to be > 0
    [SerializeField] private float pausedSpeed;
    [SerializeField] private Bounds bounds;
    [SerializeField] private GameObject ship;
    private bool paused = false;
    private int shoots = 0;
    private Animator animator;

    public void SetSpeedX(float speed) { movementSpeedX = speed; }
    public void SetPausedSpeed(float speed) { pausedSpeed = speed; }
    public void SetShip(GameObject newShip) { ship = newShip; }
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            MoveRight();

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
                transform.rotation = new Quaternion(180f, 0f, 0f, 0f);
                if (shoots > 2)
                {
                    movementSpeedX = -(movementSpeedX + movementSpeedX);
                    bounds.enabled = true;
                    transform.rotation = new Quaternion(0f, 0f, 180f, 0f);
                }
            }
        }
    }

    void MoveRight()
    {
        rb.velocity = new Vector2(movementSpeedX, 0.0f);
    }

    void ChangeAimToShip()
    {
        if (ship.transform.position.x < transform.position.x)
        {
            transform.rotation = new Quaternion(0f, 0f, 180f, 0f);
        }
        else
        {
            transform.rotation = new Quaternion(180f, 0f, 0f, 0f);
        }
    }

    void MoveInPause()
    {
        rb.velocity = new Vector2(-pausedSpeed, 0f);
    }
}
