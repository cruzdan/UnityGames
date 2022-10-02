using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    float ballSpeedX;
    float ballSpeedY;
    float maxBallSpeedY = 9f;
    private float maxSideY;
    private float minSideY;
    // Start is called before the first frame update
    void Start()
    {
        SetRandomDirection();
        maxSideY = SquaresResolution.TotalSquaresY / 2f;
        minSideY = -SquaresResolution.TotalSquaresY / 2f;
        ballSpeedX = SquaresResolution.TotalSquaresX / 2.3f;
        ballSpeedY = SquaresResolution.TotalSquaresY / 2.5f;
    }

    void SetRandomDirection() {
        int a, b;
        a = Random.Range(0, 2);
        b = Random.Range(0, 2);
        if (a == 0)
        {
            ballSpeedX *= -1;
        }
        if (b == 0)
        {
            ballSpeedY *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ballSpeedY > 0)
        {
            if (transform.position.y + transform.localScale.y / 2 + ballSpeedY * Time.deltaTime < maxSideY)
                transform.position = new Vector2(transform.position.x + ballSpeedX * Time.deltaTime, transform.position.y + ballSpeedY * Time.deltaTime);
            else
                ballSpeedY *= -1;
        }
        else
        {
            if (transform.position.y - transform.localScale.y / 2 - ballSpeedY * Time.deltaTime > minSideY)
                transform.position = new Vector2(transform.position.x + ballSpeedX * Time.deltaTime, transform.position.y + ballSpeedY * Time.deltaTime);
            else
                ballSpeedY *= -1;
        }
    }

    void Restart() {
        transform.position = new Vector2(0, 0);
        SetRandomDirection();
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            ballSpeedX *= -1;

            float diff = transform.position.y - collision.gameObject.transform.position.y;
            ballSpeedY = diff * maxBallSpeedY;

            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayCrashSound();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerWall"))
        {
            if(transform.position.x < 0f)
            {
                GameObject.Find("ScoreManager").GetComponent<Score>().IncrementScorePlayer2(1);
                Restart();
            }
            else
            {
                GameObject.Find("ScoreManager").GetComponent<Score>().IncrementScorePlayer1(1);
                Restart();
            }
        }
    }
}
