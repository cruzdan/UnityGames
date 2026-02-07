using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private AsteroidsGenerator asteroidsGenerator;
    private int cost = 0;
    private bool dead = false;
    private float bigAsteroidSize = 1;
    private AsteroidGameManager asteroidGameManager;
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
            if (asteroidGameManager == null)
                asteroidGameManager = GameObject.Find("AsteroidGameManager").GetComponent<AsteroidGameManager>();
            switch (collision.tag)
            {
                case "Player":
                    dead = true;
                    asteroidGameManager.OnShipCollided();
                    break;
                case "Bullet":
                    dead = true;
                    //it is a big asteroid
                    if (transform.localScale.magnitude > bigAsteroidSize)
                    {
                        asteroidsGenerator.Generate2Asteroids(transform.position, transform.localScale,
                            GetComponent<ForwardMovement>().GetAngle(),
                            GetComponent<SpriteRenderer>().sprite);
                        CameraShake.Instance.Shake(0.1f, 0.1f);
                    }
                    asteroidGameManager.SetMoney(asteroidGameManager.GetMoney() + cost);
                    asteroidGameManager.OnAsteroidDestroyed(collision.transform.position, cost);
                    collision.GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(collision.gameObject);
                    TimeStop.Instance.StopTime();
                    break;
            }
            SFXManager.Instance.PlaySFX(AsteroidsSFX.Instance.ExplosionClip);
            asteroidsGenerator.asteroidsPool.ReturnObjectToPool(this.gameObject);
        }
    }
}
