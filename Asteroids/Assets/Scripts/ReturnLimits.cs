using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnLimits : MonoBehaviour
{
    float firstX;
    float endX;
    float firstY;
    float endY;
    // Start is called before the first frame update
    void Start()
    {
        firstX = -Squares.totalSquaresX / 2 - transform.localScale.x / 2;
        endX = -firstX;
        firstY = Squares.totalSquaresY / 2 + transform.localScale.y / 2;
        endY = -firstY;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > firstY)
        {
            transform.position = new Vector2(transform.position.x, endY);
        }
        else if(transform.position.y < endY)
        {
            transform.position = new Vector2(transform.position.x, firstY);
        }
        if(transform.position.x > endX)
        {
            transform.position = new Vector2(firstX, transform.position.y);
        }
        else if(transform.position.x < firstX)
        {
            transform.position = new Vector2(endX, transform.position.y);
        }
    }
}
