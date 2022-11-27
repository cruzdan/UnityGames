using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float ballSpeedX;
    [SerializeField] private float ballSpeedY;
    [SerializeField] private float maxBallSpeedY = 9f;
    Rigidbody2D rb;
    Vector2 movement;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballSpeedX = SquaresResolution.TotalSquaresX / 2.3f;
        ballSpeedY = SquaresResolution.TotalSquaresY / 2.5f;
        SetRandomDirection();
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

    void FixedUpdate()
    {
        movement.x = transform.position.x + ballSpeedX * Time.fixedDeltaTime;
        movement.y = transform.position.y + ballSpeedY * Time.fixedDeltaTime;
        rb.MovePosition(movement);
    }

    void Restart() {
        transform.position = new Vector2(0, 0);
        SetRandomDirection();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerWall":
                ballSpeedY *= -1;
                break;
            case "BoundWall":
                if (transform.position.x < 0f)
                {
                    GameObject.Find("ScoreManager").GetComponent<Score>().IncrementScorePlayer2(1);
                    Restart();
                }
                else
                {
                    GameObject.Find("ScoreManager").GetComponent<Score>().IncrementScorePlayer1(1);
                    Restart();
                }
                break;
            case "Paddle":
                ballSpeedX *= -1;

                float diff = transform.position.y - collision.gameObject.transform.position.y;
                ballSpeedY = diff * maxBallSpeedY;

                GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayCrashSound();
                break;
        }
    }
}
