using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovementRB : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speedX;
    private float speedY;

    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(float newSpeedX, float newSpeedY)
    {
        speedX = newSpeedX;
        speedY = newSpeedY;
        rb.velocity = new Vector2(speedX, speedY);
    }
    //angle in radians
    public void SetSpeedByAngle(float angle, float speed)
    {
        speedX = Mathf.Cos(angle) * speed;
        speedY = Mathf.Sin(angle) * speed;
        rb.velocity = new Vector2(speedX, speedY);
    }
}
