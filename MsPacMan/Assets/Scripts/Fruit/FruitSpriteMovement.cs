using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpriteMovement : MonoBehaviour
{
    [SerializeField] private Transform fruit;
    [SerializeField] private float speed;
    [SerializeField] private Transform limitUp;
    [SerializeField] private Transform limitDown;
    void Update()
    {
        if (speed > 0)
        {
            if(fruit.position.y >= limitUp.position.y)
            {
                speed *= -1;
            }
        }
        else
        {
            if(fruit.position.y <= limitDown.position.y)
            {
                speed *= -1;
            }
            
        }
        Move();
    }
    void Move()
    {
        fruit.position = new Vector2(fruit.position.x, fruit.position.y + speed * Time.deltaTime);
    }
}
