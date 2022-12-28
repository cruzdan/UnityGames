using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private GameObject ball;
    private float currentSpeed;
    private float maxSpeed = 7;
    private Rigidbody2D rb;
    Vector2 velocity = Vector2.zero;
    bool canMove = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    void Update()
    {
        rb.velocity = Vector2.zero;
        if (canMove)
        {
            currentSpeed = (ball.transform.position.y - transform.position.y) * 4;
            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
            if (currentSpeed < -maxSpeed)
            {
                currentSpeed = -maxSpeed;
            }
            velocity.y = currentSpeed;
            rb.velocity = velocity;
        }
    }

    public void SetMove(bool move)
    {
        canMove = move;
    }

    public void RestartPaddle() {
        transform.position = new Vector2(transform.position.x, 0);
        rb.velocity = Vector2.zero;
    }
}
