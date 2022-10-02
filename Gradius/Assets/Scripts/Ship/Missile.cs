using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;

    //0 -> Horizontal, 1 -> Falling
    [SerializeField] private Sprite[] sprites;
    private Rigidbody2D rb;
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    //0 -> Horizontal, 1 -> Falling
    private int state = 1;

    //bounds
    private float endHorizontal = 0;
    private float endVertical = 0;
    public void SetSpeedX(float speed) { speedX = speed; }
    public void SetSpeedY(float speed) { speedY = speed; }
    public void SetEnemyManager(EnemyManager e) { enemyManager = e; }
    public void SetState(int newState) { state = newState; GetComponent<SpriteRenderer>().sprite = sprites[state]; }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        endHorizontal = SquaresResolution.TotalSquaresX / 2.0f + SpriteBounds.GetSpriteWidth(gameObject) / 2;
        endVertical = -(0.78f * SquaresResolution.TotalSquaresY / 2.0f) - SpriteBounds.GetSpriteHeight(gameObject) / 2;
    }

    private void Update()
    {
        if (endHorizontal < 0)
        {
            if (transform.position.x < endHorizontal)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (transform.position.x > endHorizontal)
            {
                gameObject.SetActive(false);
            }
        }
        if (endVertical < 0)
        {
            if (transform.position.y < endVertical)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (transform.position.y > endVertical)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speedX, speedY);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.transform.tag;
        if (state == 1)
        {
            if (tag.Equals("Floor") || tag.Equals("Mountain"))
            {
                state = 0;
                GetComponent<SpriteRenderer>().sprite = sprites[state];
            }
        }
        switch (tag)
        {
            case "Enemy4":
            case "Enemy5":
            case "Enemy5up":
                EnemyInfo enemy = collision.gameObject.GetComponent<EnemyInfo>();
                enemy.SetDead(true);
                if (enemy.GetUpgrade())
                {
                    enemyManager.GenerateUpgrade(collision.transform.position.x, collision.transform.position.y);
                }
                GetComponent<CollisionBulletToEnemy>().SetDead(true);
                collision.gameObject.GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(collision.gameObject);
                gameObject.SetActive(false);
                break;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(state == 0)
        {
            string tag = collision.transform.tag;
            if (tag.Equals("Floor") || tag.Equals("Mountain"))
            {
                state = 1;
                GetComponent<SpriteRenderer>().sprite = sprites[state];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }
}
