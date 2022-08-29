using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    //0 -> Horizontal, 1 -> Falling
    [SerializeField] private Sprite[] sprites;
    private Rigidbody2D rb;
    private float speedX = 200.0f;
    private float speedY = -200.0f;
    //0 -> Horizontal, 1 -> Falling
    private int state = 1;
    
    //ship and option info
    private int id;
    private Ship ship;

    //bounds
    private float endHorizontal = 0;
    private float endVertical = 0;

    public void SetShip(Ship newShip) { ship = newShip; }
    public Ship GetShip() { return ship; }
    public void SetID(int newId) { id = newId; }
    public int GetID() { return id; }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        endHorizontal = Squares.totalSquaresX / 2.0f + SpriteBounds.GetSpriteWidth(gameObject) / 2;
        endVertical = -(0.78f * Squares.totalSquaresY / 2.0f) - SpriteBounds.GetSpriteHeight(gameObject) / 2;
    }

    private void Update()
    {
        if (endHorizontal < 0)
        {
            if (transform.position.x < endHorizontal)
            {
                ship.ActiveMissile(id);
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (transform.position.x > endHorizontal)
            {
                ship.ActiveMissile(id);
                Destroy(this.gameObject);
            }
        }
        if (endVertical < 0)
        {
            if (transform.position.y < endVertical)
            {
                ship.ActiveMissile(id);
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (transform.position.y > endVertical)
            {
                ship.ActiveMissile(id);
                Destroy(this.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speedX * Time.fixedDeltaTime, speedY * Time.fixedDeltaTime);
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
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.SetDead(true);
                GetComponent<CollisionBulletToEnemy>().SetDead(true);
                ActiveShipMissile();
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
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

    public void ActiveShipMissile()
    {
        ship.ActiveMissile(id);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            ActiveShipMissile();
            Destroy(this.gameObject);
        }
    }
}
