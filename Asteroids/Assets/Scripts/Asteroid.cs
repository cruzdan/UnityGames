using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private int cost = 0;

    public void SetCost(int newCost)
    {
        cost = newCost;
    }

    public int GetCost()
    {
        return cost;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AsteroidGameManager man = GameObject.Find("AsteroidGameManager").GetComponent<AsteroidGameManager>();
        switch (collision.tag)
        {
            case "Player":
                man.Restart();
                break;
            case "Bullet":
                //it is a big asteroid
                if (transform.localScale.magnitude > Squares.totalSquaresInclined / 20.0f)
                {
                    GameObject.Find("AsteroidGenerator").GetComponent<AsteroidsGenerator>().Generate2Asteroids(
                        transform.position, transform.localScale, GetComponent<ForwardMovement>().GetAngle(),
                        GetComponent<SpriteRenderer>().sprite);
                }
                man.SetMoney(man.GetMoney() + cost);
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
                break;
        }
    }
}
