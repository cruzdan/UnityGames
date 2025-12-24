using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private AsteroidsGenerator asteroidsGenerator;
    private int cost = 0;
    bool dead = false;
    public void SetAsteroidsGenerator(AsteroidsGenerator ast)
    {
        asteroidsGenerator = ast;
    }
    public void SetDead(bool value) { dead = value; }
    public bool HasAsteroidsGenerator()
    {
        return asteroidsGenerator != null;
    }
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
        if (!dead)
        {
            AsteroidGameManager man = GameObject.Find("AsteroidGameManager").GetComponent<AsteroidGameManager>();
            switch (collision.tag)
            {
                case "Player":
                    dead = true;
                    man.Restart();
                    break;
                case "Bullet":
                    dead = true;
                    //it is a big asteroid
                    if (transform.localScale.magnitude > SquaresResolution.TotalSquaresInclined / 20.0f)
                    {
                        asteroidsGenerator.Generate2Asteroids(transform.position, transform.localScale,
                            GetComponent<ForwardMovement>().GetAngle(),
                            GetComponent<SpriteRenderer>().sprite);
                    }
                    man.SetMoney(man.GetMoney() + cost);
                    collision.GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(collision.gameObject);
                    asteroidsGenerator.asteroidsPool.ReturnObjectToPool(this.gameObject);
                    break;
            }
        }
    }
}
