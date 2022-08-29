using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy7 : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    [SerializeField] private Transform ship;
    [SerializeField] private ShootToShip shootToShip;
    private bool paused = false;
    Rigidbody2D rb;
    public void SetSpeedX(float speed) { speedX = speed; }
    public void SetSpeedY(float newSpeedY) { speedY = newSpeedY; }
    public void SetShip(Transform newShip) { ship = newShip; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speedX, speedY);
    }

    private void FixedUpdate()
    {
        if(!paused)
        {
            if(rb.velocity.y > 0)
            {
                if (transform.position.y >= ship.position.y)
                {
                    paused = true;
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    shootToShip.enabled = true;
                }
            }
            else
            {
                if (transform.position.y <= ship.position.y)
                {
                    paused = true;
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    shootToShip.enabled = true;
                }
            }
        }
    }
}