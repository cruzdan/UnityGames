using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode up;
    public KeyCode down;
    bool canMove = false;
    [SerializeField] private float speed;
    Vector2 velocity = Vector2.zero;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 3;
    }

    void Update()
    {
        if (canMove) {
            if (Input.GetKey(up))
            {
                velocity.y = speed;
                rb.velocity = velocity;
            }
            if (Input.GetKey(down))
            {
                velocity.y = -speed;
                rb.velocity = velocity;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void SetMove(bool move) {
        canMove = move;
    } 
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void RestartPaddle() {
        transform.position = new Vector2(transform.position.x, 0);
        rb.velocity = Vector2.zero;
    }
}
