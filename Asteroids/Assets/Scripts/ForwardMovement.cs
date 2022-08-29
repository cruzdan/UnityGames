using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    private float speedX;
    private float speedY;
    private float angleZ;
    public void Init(float speed, float angle)
    {
        float mAngle = angle * Mathf.Deg2Rad;
        speedX = Mathf.Cos(mAngle) * speed;
        speedY = Mathf.Sin(mAngle) * speed;
        angleZ = angle;
    }

    public void Init(float newSpeedX, float newSpeedY, float angle)
    {
        float mAngle = angle * Mathf.Deg2Rad;
        speedX = Mathf.Cos(mAngle) * newSpeedX;
        speedY = Mathf.Sin(mAngle) * newSpeedY;
        angleZ = angle;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(
            transform.position.x + speedX * Time.deltaTime,
            transform.position.y + speedY * Time.deltaTime
        );
    }

    public float GetAngle()
    {
        return angleZ;
    }
}
