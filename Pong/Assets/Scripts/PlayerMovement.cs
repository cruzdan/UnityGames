using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public KeyCode up, down; 
    float actualSpeed=0;
    const float MAXSPEED = 3f;
    const float decrementSpeed = 0.05f;
    const float incrementSpeed = 0.05f;
    bool canMove = false;

    // Update is called once per frame
    void Update()
    {
        if (canMove) { 
            bool pressed = false;
            if (Input.GetKey(up))
            {
                if (actualSpeed < MAXSPEED)
                {
                    actualSpeed = actualSpeed + incrementSpeed;
                    pressed = true;
                }
                if (transform.position.y + transform.localScale.y / 2 + speed * Time.deltaTime < 5)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
                }
            }
            else if (Input.GetKey(down))
            {
                if (actualSpeed > -MAXSPEED)
                {
                    actualSpeed = actualSpeed - incrementSpeed;
                    pressed = true;
                }
                if (transform.position.y - transform.localScale.y / 2 - speed * Time.deltaTime > -5)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
                }
            }
            if (!pressed && actualSpeed != 0)
            {
                if (actualSpeed > 0)
                {
                    if (actualSpeed - decrementSpeed > 0)
                    {
                        actualSpeed -= decrementSpeed;
                    }
                    else
                    {
                        actualSpeed = 0;
                    }
                }
                else
                {
                    if (actualSpeed + incrementSpeed < 0)
                    {
                        actualSpeed += incrementSpeed;
                    }
                    else
                    {
                        actualSpeed = 0;
                    }
                }
            }
            transform.position = new Vector2(transform.position.x, transform.position.y + actualSpeed * Time.deltaTime);
        }
    }

    public void SetMove(bool move) {
        canMove = move;
    } 

    public void RestartPaddle() {
        transform.position = new Vector2(transform.position.x, 0);
        actualSpeed = 0;
    }
}
