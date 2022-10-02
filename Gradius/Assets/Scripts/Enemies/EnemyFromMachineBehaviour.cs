using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Moves the object on the horizontal side and the vertical side until it reaches the ship position y, when it reaches that, 
 it enables the ShootToShip script*/
public class EnemyFromMachineBehaviour : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    [SerializeField] private Transform ship;
    [SerializeField] private ShootToShip shootToShip;
    private bool paused = false;
    Rigidbody2D rb;
    public void SetSpeedX(float speed) { speedX = speed; }
    public void SetSpeedY(float speed) { speedY = speed; }
    public void SetShip(Transform newShip) { ship = newShip; }
    public void SetPaused(bool value) { paused = value; }

    public void InitRigidBody()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitVelocity()
    {
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