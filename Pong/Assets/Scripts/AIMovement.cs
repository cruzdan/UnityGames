using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private GameObject ball;
    public float speed = 5.2f;
    bool canMove = false;
   
    public void Init()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // NOTE(isaveg): Try alternative solution getting the movement through the  rigidbody component and detect screen borders with a simple collider
    // Update is called once per frame
    void Update()
    {
        if (canMove) {
            if (ball.transform.position.y > transform.position.y)
            {
                if (transform.position.y + transform.localScale.y / 2 + speed * Time.deltaTime < 5)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
                }
            }
            else
            {
                if (transform.position.y - transform.localScale.y / 2 - speed * Time.deltaTime > -5)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
                }
            }
        }
    }

    public void SetMove(bool move)
    {
        canMove = move;
    }

    public void RestartPaddle() {
        transform.position = new Vector2(transform.position.x, 0);
    }
}
