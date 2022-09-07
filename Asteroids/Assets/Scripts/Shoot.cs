using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private PauseManager pManager;
    private GameObject bullet;
    public float timeToShoot = 0.001f;
    float timer = 0;
    public int bulletsToShoot = 10;

    // Update is called once per frame
    void Update()
    {
        if (!pManager.pause)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (timer <= 0)
                {
                    GenerateBullet();
                    timer = timeToShoot;
                }
            }
            if (timer > 0)
                timer -= Time.deltaTime;
        }
        
    }

    void GenerateBullet()
    {
        float width = transform.localScale.x / 3.0f;
        float height = transform.localScale.y / 2.0f;
        float posX = transform.position.x + transform.up.normalized.x * transform.localScale.x / 2.0f + transform.up.normalized.x * width / 2.0f;
        float posY = transform.position.y + transform.up.normalized.y * transform.localScale.y / 2.0f + transform.up.normalized.y * height / 2.0f;
        //space between bullets
        float sideX = transform.right.normalized.x * width;
        float sideY = transform.right.normalized.y * height;
        switch (bulletsToShoot)
        {
            case 1:
                GenerateBullet(posX, posY, width, height);
                break;
            case 2:
                posX -= sideX;
                posY -= sideY;
                GenerateBullet(posX, posY, width, height);
                posX += sideX;
                posY += sideY; 
                GenerateBullet(posX, posY, width, height);
                break;
            case 3:
                GenerateBullet(posX, posY, width, height);
                GenerateBullet(posX - sideX, posY - sideY, width, height);
                GenerateBullet(posX + sideX, posY + sideY, width, height);
                break;
        }
    }

    void GenerateBullet(float posX, float posY, float width, float height)
    {
        // TODO(isaveg): Bullets are instantiated every tick, instead there shuold be a memory pool and we reuse it
        //               an opportunity to learn how to avoid GC and reuse memory
        bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = new Vector2(posX, posY);
        bullet.transform.localScale = new Vector2(width, height);
        bullet.transform.eulerAngles = transform.eulerAngles;
        bullet.gameObject.GetComponent<ForwardMovement>().Init(Squares.totalSquaresX / 2.0f, Squares.totalSquaresY / 1.5f, transform.eulerAngles.z + 90.0f);
        bullet.gameObject.GetComponent<Bounds>().Init(transform.eulerAngles.z + 90.0f, width, height);
    }

    public void Restart()
    {
        timer = 0;
    }
}
