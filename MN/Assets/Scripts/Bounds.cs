using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    [SerializeField] private float boundX;
    //0 -> left, 1 -> right
    [SerializeField] private int direction;

    public void SetBoundX(float bound)
    {
        boundX = bound;
    }
    public void SetDirection(int newDirection)
    {
        direction = newDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if(direction == 0)
        {
            if(transform.position.x < boundX)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (transform.position.x > boundX)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
