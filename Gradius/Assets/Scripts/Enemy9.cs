using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy9 : MonoBehaviour
{
    [SerializeField] private Transform ship;
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    [SerializeField] private float timeToShoot;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Level level;
    [SerializeField] private int total;
    [SerializeField] private float limitYUp;
    [SerializeField] private float limitYDown;
    private GameObject bullet;
    private float timer = 0f;
    private bool stop = false;
    Rigidbody2D rb;

    public void SetShip(Transform newShip) { ship = newShip; }
    public void SetSpeedX(float speed) { speedX = speed; }
    public void SetSpeedY(float speed) { speedY = speed; }
    public void SetTimeToShoot(float time) { timeToShoot = time; }
    public void SetLevel(Level newLevel) { level = newLevel; }
    public void SetTotal(int value) { total = value; }
    public void SetLimitYUp(float limit) { limitYUp = limit; }
    public void SetLimitYDown(float limit) { limitYDown = limit; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!stop)
        {
            rb.velocity = new Vector2(speedX, 0f);
            if (transform.position.x < Squares.totalSquaresX / 2f - Squares.totalSquaresX / 3f)
            {
                stop = true;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ShootEnemyBullet();
                timer = timeToShoot;
            }
            if (total < 2)
            {
                
                if (ship.position.y < transform.position.y)
                {
                    rb.velocity = new Vector2(0f, -speedY);
                }
                else
                {
                    rb.velocity = new Vector2(0f, speedY);
                }
            }
            else
            {
                rb.velocity = new Vector2(0f, speedY);
                if (speedY > 0)
                {
                    if (transform.position.y > limitYUp)
                    {
                        speedY = -speedY;
                    }
                }
                else
                {
                    if (transform.position.y < limitYDown)
                    {
                        speedY = -speedY;
                    }
                }
            }
        }
    }

    void ShootEnemyBullet()
    {

        BoxCollider2D enemyCollider = GetComponent<BoxCollider2D>();
        float posX;
        float posY;

        posX = transform.position.x - enemyCollider.size.x / 2f;
        posY = transform.position.y + enemyCollider.size.y / 2f - enemyCollider.size.y * 0.0833f;
        GenerateBullet(-Squares.totalSquaresX, posX, posY);

        posY = transform.position.y + enemyCollider.size.y / 2f - enemyCollider.size.y * 0.8333f;
        GenerateBullet(-Squares.totalSquaresX, posX, posY);

        posX -= enemyCollider.size.x / 3f;
        posY = transform.position.y + enemyCollider.size.y / 2f - enemyCollider.size.y * 0.3125f;
        GenerateBullet(-Squares.totalSquaresX, posX, posY);

        posY = transform.position.y + enemyCollider.size.y / 2f - enemyCollider.size.y * 0.6041f;
        GenerateBullet(-Squares.totalSquaresX, posX, posY);
    }

    void GenerateBullet(float speed, float posX, float posY)
    {
        bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.GetComponent<Bounds>().Init(180f, SpriteBounds.GetSpriteWidth(bullet), SpriteBounds.GetSpriteHeight(bullet));
        bullet.GetComponent<ForwardMovement>().Init(speed, 0f);
        bullet.transform.position = new Vector2(posX,
            posY);
    }

    public void Dead()
    {
        level.SetActualEnemies(0);
    }
}
