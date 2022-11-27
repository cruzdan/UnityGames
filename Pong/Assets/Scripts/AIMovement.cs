using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private GameObject ball;
    [SerializeField] private float speed = 6f;
    private Rigidbody2D rb;
    Vector2 velocity;
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
            if (ball.transform.position.y > transform.position.y + 0.5f)
            {
                velocity.y = speed;
                rb.velocity = velocity;
            }
            else if (ball.transform.position.y < transform.position.y - 0.5f)
            {
                velocity.y = -speed;
                rb.velocity = velocity;
            }
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
