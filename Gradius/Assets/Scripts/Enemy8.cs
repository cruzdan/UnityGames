using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy8 : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private float forceY;
    private Rigidbody2D rb;
    public void SetSpeedX(float speed) { speedX = speed; }
    public void SetForceY(float force) { forceY = force; }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speedX, forceY);
    }
}