using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //13 is the floor layer
        if(collision.gameObject.layer == 13)
        {
            rb.velocity = new Vector2(rb.velocity.x, Squares.totalSquaresY);
        }
    }
}
