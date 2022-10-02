using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode up, down; 
    float actualSpeed = 0;
    const float MAXSPEED = 4f;
    const float decrementSpeed = 0.2f;
    const float incrementSpeed = 0.3f;
    bool canMove = false;
    float limitY;
    private void Start()
    {
        limitY = SquaresResolution.TotalSquaresY / 2f - transform.localScale.y / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) { 
            bool pressed = false;
            if (Input.GetKey(up))
            {
                if (actualSpeed < MAXSPEED)
                {
                    actualSpeed = actualSpeed + incrementSpeed * Time.deltaTime;
                    pressed = true;
                }

            }
            else if (Input.GetKey(down))
            {
                if (actualSpeed > -MAXSPEED)
                {
                    actualSpeed = actualSpeed - incrementSpeed * Time.deltaTime;
                    pressed = true;
                }

            }
            if (!pressed && actualSpeed != 0)
            {
                if (actualSpeed > 0)
                {
                    if (actualSpeed - decrementSpeed * Time.deltaTime > 0)
                    {
                        actualSpeed -= decrementSpeed * Time.deltaTime;
                    }
                    else
                    {
                        actualSpeed = 0;
                    }
                }
                else
                {
                    if (actualSpeed + incrementSpeed * Time.deltaTime < 0)
                    {
                        actualSpeed += incrementSpeed * Time.deltaTime;
                    }
                    else
                    {
                        actualSpeed = 0;
                    }
                }
            }
            if(transform.position.y + actualSpeed < limitY && transform.position.y + actualSpeed > -limitY)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + actualSpeed);
            }
            else
            {
                actualSpeed = 0;
            }
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
