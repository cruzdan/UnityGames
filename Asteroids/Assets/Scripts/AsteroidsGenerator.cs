using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    private GameObject asteroid;
    float timer = 0;
    float timeToGenerateAsteroid = 1.0f;
    float firstX;
    float firstY;
    float endX;
    float endY;
    int bonus = 0;

    private void Start()
    {
        firstX = -Squares.totalSquaresX / 2.0f - transform.localScale.x / 2;
        endX = -firstX;
        firstY = Squares.totalSquaresY / 2.0f + transform.localScale.y / 2;
        endY = -firstY;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            GenerateAsteroid();
            timer = timeToGenerateAsteroid;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    void GenerateAsteroid()
    {
        float width = Random.Range(Squares.totalSquaresX / 20.0f, Squares.totalSquaresX / 10.0f);
        float height = Random.Range(Squares.totalSquaresY / 20.0f, Squares.totalSquaresY / 10.0f);
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
        asteroid = Instantiate(asteroidPrefab) as GameObject;
        asteroid.transform.position = new Vector3(x, y, 0f);
        asteroid.transform.localScale = new Vector3(width, height, 1f);
        asteroid.GetComponent<Asteroid>().SetCost(GetAsteroidCost(asteroid.transform.localScale.magnitude) + bonus);
        asteroid.GetComponent<ForwardMovement>().Init(Squares.totalSquaresX / 4.0f, Squares.totalSquaresY / 3.0f, angle);
        asteroid.GetComponent<Bounds>().Init(angle, width, height);
    }
    
    public void Generate2Asteroids(Vector3 originalPos, Vector3 originalScale, float angle, Sprite spr)
    {
        Vector3 newScale = originalScale / 2;
        float newAngle = angle - 45.0f;
        int cost = GetAsteroidCost(newScale.magnitude);

        asteroid = Instantiate(asteroidPrefab) as GameObject;
        asteroid.transform.position = originalPos;
        asteroid.transform.localScale = newScale;
        asteroid.GetComponent<ForwardMovement>().Init(Squares.totalSquaresX / 4.0f, Squares.totalSquaresY / 3.0f, newAngle);
        asteroid.GetComponent<Bounds>().Init(newAngle, newScale.x, newScale.y);
        asteroid.GetComponent<Asteroid>().SetCost(cost);
        asteroid.GetComponent<SpriteRenderer>().sprite = spr;


        asteroid = Instantiate(asteroidPrefab) as GameObject;
        asteroid.transform.position = originalPos;
        asteroid.transform.localScale = newScale;
        asteroid.GetComponent<ForwardMovement>().Init(Squares.totalSquaresX / 4.0f, Squares.totalSquaresY / 3.0f, newAngle + 90.0f);
        asteroid.GetComponent<Bounds>().Init(newAngle + 90.0f, newScale.x, newScale.y);
        asteroid.GetComponent<Asteroid>().SetCost(cost);
        asteroid.GetComponent<SpriteRenderer>().sprite = spr;
    }

    //big asteroid = 2, little asteroid = 1
    int GetAsteroidCost(float magnitude)
    {
        if (magnitude > Squares.totalSquaresInclined / 20.0f)
            return 2;
        return 1;
    }

    public void SetBonus(int newBonus)
    {
        bonus = newBonus;
    }
    
}
