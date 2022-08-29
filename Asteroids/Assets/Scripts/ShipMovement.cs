using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    Rigidbody2D body;
    float originalSpeed;
    float originalAngularSpeed;
    public float speed;
    public float maxSpeed;
    //stop the ship with time
    public float drag = 1.0f;
    public float angularSpeed = 3.0f;
    bool move;
    float rotate;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.drag = drag;

        speed = Squares.totalSquaresInclined / 3.0f;
        originalSpeed = speed;
        maxSpeed = 5.0f * speed;

        originalAngularSpeed = angularSpeed;
    }

    public void InitSpeed()
    {
        speed = originalSpeed;
    }

    public void InitRotation()
    {
        angularSpeed = originalAngularSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetKey(KeyCode.W);
        rotate = Input.GetAxis("Horizontal");
        Rotate();
    }

    private void FixedUpdate()
    {
        if (move)
        {
            body.AddForce(transform.up * speed);
            if(body.velocity.magnitude > maxSpeed)
            {
                body.velocity = body.velocity.normalized * maxSpeed;
            }
        }
    }

    private void Rotate()
    {
        if(rotate != 0)
        {
            transform.Rotate(0, 0, -angularSpeed * rotate * Time.deltaTime);
        }
    }

    public void Restart()
    {
        transform.position = Vector2.zero;
        transform.eulerAngles = Vector3.zero;
        body.velocity = Vector2.zero;
    }

    public void SetSpeedByPercentage(float percentage)
    {
        float mul = percentage / 100.0f;
        speed = originalSpeed * mul;
    }

    public void SetAngularSpeedByPercentage(float percentage)
    {
        float mul = percentage / 100.0f;
        angularSpeed = originalAngularSpeed * mul;
    }
}
