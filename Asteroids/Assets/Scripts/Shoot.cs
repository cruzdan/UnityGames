using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private PauseManager pManager;
    [SerializeField] private ObjectPool bulletsPool;
    private GameObject bullet;
    public float timeToShoot = 0.25f;
    float timer = 0;
    //min: 1, max: 3
    public int bulletsToShoot = 1;

    //auxiliar variables to generate new Bullets
    BoundsPoolObject bound;

    // Update is called once per frame
    void Update()
    {
        if (!pManager.pause)
        {
            if (Input.GetButton("Fire1"))
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
                posX -= sideX / 2f;
                posY -= sideY / 2f;
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
        bullet = bulletsPool.GetObjectFromPool();
        if(bullet != null)
        {
            bullet.transform.position = new Vector2(posX, posY);
            bullet.transform.localScale = new Vector2(width, height);
            bullet.transform.eulerAngles = transform.eulerAngles;
            bullet.GetComponent<ForwardMovement>().Init(SquaresResolution.TotalSquaresX / 2.0f, 
                SquaresResolution.TotalSquaresY / 1.5f, transform.eulerAngles.z + 90.0f);
            bound = bullet.GetComponent<BoundsPoolObject>();
            bound.Init(transform.eulerAngles.z + 90.0f, width, height);
            bullet.GetComponent<SpriteRenderer>().sprite = bulletPrefab.GetComponent<SpriteRenderer>().sprite;
            if (!bound.HasObjectPool())
            {
                bound.SetObjectPool(bulletsPool);
            }
        }
    }

    public void Restart()
    {
        timer = 0;
    }
}
