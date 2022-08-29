using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float ballSpeedX;
    public float ballSpeedY = 4;
    float maxBallSpeedY = 9f;
    // Start is called before the first frame update
    void Start()
    {
        SetRandomSpeed();
    }

    void SetRandomSpeed() {
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
            if (transform.position.y + transform.localScale.y / 2 + ballSpeedY * Time.deltaTime < 5)
                transform.position = new Vector2(transform.position.x + ballSpeedX * Time.deltaTime, transform.position.y + ballSpeedY * Time.deltaTime);
            else
                ballSpeedY *= -1;
        }
        else
        {
            if (transform.position.y - transform.localScale.y / 2 - ballSpeedY * Time.deltaTime > -5)
                transform.position = new Vector2(transform.position.x + ballSpeedX * Time.deltaTime, transform.position.y + ballSpeedY * Time.deltaTime);
            else
                ballSpeedY *= -1;
        }

        if (transform.position.x - transform.localScale.x / 2 < -12)
        {
            GameObject.Find("ScoreManager").GetComponent<Score>().IncrementScorePlayer2(1);
            Restart();
        }
        else if (transform.position.x + transform.localScale.x / 2 > 12)
        {
            GameObject.Find("ScoreManager").GetComponent<Score>().IncrementScorePlayer1(1);
            Restart();
        }
    }

    void Restart() {
        transform.position = new Vector2(0, 0);
        SetRandomSpeed();
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
}
