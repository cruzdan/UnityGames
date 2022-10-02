using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private PauseManager pauseManager;
    public ObjectPool asteroidsPool;
    private GameObject asteroid;
    //private int MaxPoolSize = 45;
    //private Stack<GameObject> inactiveAsteroids = new Stack<GameObject>();
    float timer = 0;
    float timeToGenerateAsteroid = 1.0f;
    float firstX;
    float firstY;
    float endX;
    float endY;
    int bonus = 0;

    //auxiliar variables to generate new Asteroids
    Asteroid ast;
    BoundsPoolObject bound;

    private void Start()
    {
        firstX = -SquaresResolution.TotalSquaresX / 2.0f - transform.localScale.x / 2;
        endX = -firstX;
        firstY = SquaresResolution.TotalSquaresY / 2.0f + transform.localScale.y / 2;
        endY = -firstY;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseManager.pause)
        {
            if (timer <= 0)
            {
                GenerateRandomAsteroid();
                timer = timeToGenerateAsteroid;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }
   
    void GenerateRandomAsteroid()
    {
        float width = Random.Range(SquaresResolution.TotalSquaresX / 20.0f, SquaresResolution.TotalSquaresX / 10.0f);
        float height = Random.Range(SquaresResolution.TotalSquaresY / 20.0f, SquaresResolution.TotalSquaresY / 10.0f);
        float angle = Random.Range(0.0f, 90.0f);
        float x = 0, y = 0;

        switch (Random.Range(0, 4))
        {
            case 0:
                //superior
                x = Random.Range(firstX, endX);
                y = firstY + height / 2;
                angle += 225f;
                break;
            case 1:
                //left
                x = firstX - width / 2;
                y = Random.Range(endY, firstY);
                angle += 315f;
                break;
            case 2:
                //inferior
                x = Random.Range(firstX, endX);
                y = endY - height / 2;
                angle += 45f;
                break;
            case 3:
                //right
                x = endX + width / 2;
                y = Random.Range(endY, firstY);
                angle += 135f;
                break;
        }

        Vector2 position = new(x, y);
        Vector2 scale = new(width, height);
        GenerateAsteroid(position, scale, angle, GetAsteroidCost(scale.magnitude) + bonus, 
            asteroidPrefab.GetComponent<SpriteRenderer>().sprite);
    }
    
    public void Generate2Asteroids(Vector3 originalPos, Vector3 originalScale, float angle, Sprite spr)
    {
        Vector3 newScale = originalScale / 2;
        float newAngle = angle - 45.0f;
        int cost = GetAsteroidCost(newScale.magnitude);

        GenerateAsteroid(originalPos, newScale, newAngle, cost, spr);
        GenerateAsteroid(originalPos, newScale, newAngle + 90.0f, cost, spr);
    }

    void GenerateAsteroid(Vector2 position, Vector2 scale, float angle, int cost, Sprite spr)
    {
        asteroid = asteroidsPool.GetObjectFromPool();
        if(asteroid != null)
        {
            ast = asteroid.GetComponent<Asteroid>();
            bound = asteroid.GetComponent<BoundsPoolObject>();
            asteroid.transform.position = position;
            asteroid.transform.localScale = scale;
            asteroid.GetComponent<ForwardMovement>().Init(SquaresResolution.TotalSquaresX / 4.0f, 
                SquaresResolution.TotalSquaresY / 3.0f, angle);
            bound.Init(angle, scale.x, scale.y);
            ast.SetCost(cost);

            if (!ast.HasAsteroidsGenerator())
            {
                ast.SetAsteroidsGenerator(this);
            }
            if (!bound.HasObjectPool())
            {
                bound.SetObjectPool(asteroidsPool);
            }
            if (spr != null)
                asteroid.GetComponent<SpriteRenderer>().sprite = spr;
        }
    }

    //big asteroid = 2, little asteroid = 1
    int GetAsteroidCost(float magnitude)
    {
        if (magnitude > SquaresResolution.TotalSquaresInclined / 20.0f)
            return 2;
        return 1;
    }

    public void SetBonus(int newBonus)
    {
        bonus = newBonus;
    }
    
}
