using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemy0Prefab;
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;
    [SerializeField] private GameObject enemy2RedPrefab;
    [SerializeField] private GameObject enemy3Prefab;
    [SerializeField] private GameObject enemy4Prefab;
    [SerializeField] private GameObject enemy4RedPrefab;
    [SerializeField] private GameObject enemy5DownPrefab;
    [SerializeField] private GameObject enemy5DownRedPrefab;
    [SerializeField] private GameObject enemy5UpPrefab;
    [SerializeField] private GameObject enemy5UpRedPrefab;
    [SerializeField] private GameObject enemy6Prefab;
    [SerializeField] private GameObject enemy7Prefab;
    [SerializeField] private GameObject enemy8Prefab;
    [SerializeField] private GameObject enemy9Prefab;
    [SerializeField] private GradiusManager gradiusManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Level level;
    [SerializeField] private Information info;

    private GameObject enemy;

    private float menuWidth;
    private float menuHeight;
    int total;

    private void Start()
    {
        total = PlayerVariables.Instance.GetPlayers();
        menuWidth = Squares.totalSquaresX;
        menuHeight = Squares.totalSquaresY * 0.89f;
    }

    public void CheckEnemyData(Collider2D collision)
    {
        EnemyData data = collision.GetComponent<EnemyData>();
        int type = data.GetEnemyType();
        
        switch (type)
        {
            case 0:
                GenerateEnemies0(GetUp(data.transform.position.y));
                break;
            case 1:
                GenerateEnemies1(GetUp(data.transform.position.y));
                break;
            case 2:
                GenerateEnemies2(data.GetUpgrade());
                break;
            case 3:
                GenerateEnemy3(GetUp(data.transform.position.y));
                break;
            case 4:
                GenerateEnemy4(data.GetUpgrade());
                break;
            case 5:
                GenerateEnemy5(data.GetUpgrade(), GetUp(data.transform.position.y));
                break;
            case 6:
                GenerateEnemy6(GetUp(data.transform.position.y));
                break;
        }
    }

    //if the position y is > 0 return true, false in other case
    bool GetUp(float posY)
    {
        if (posY > 0)
            return true;
        return false;
    }
    void GenerateEnemies0(bool up)
    {
        //total enemies in line
        int total = 4;
        for (int i = 0; i < total; i++)
        {
            enemy = Instantiate(enemy0Prefab) as GameObject;
            enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);
            float positionY;
            float positionX;
            if (up)
            {
                positionY = Squares.totalSquaresY / 2f - menuHeight * 0.15f;
            }
            else
            {
                positionY = Squares.totalSquaresY / 2f - menuHeight * 0.85f;
            }

            BoxCollider2D collider = enemy.GetComponent<BoxCollider2D>();
            positionX = menuWidth / 2f + collider.size.x * (1.5f * i + 0.5f);
            enemy.transform.position = new Vector2(positionX, positionY);

            enemy.GetComponent<Bounds>().Init(180f, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));

            Enemy e = enemy.GetComponent<Enemy>();
            e.SetLifes(1);
            e.SetScore(100);
            e.SetInformation(info);

            ForwardMovementRB forward = enemy.GetComponent<ForwardMovementRB>();
            forward.Init();
            forward.SetSpeed(-menuWidth * 0.4166f, 0f);

            Enemy0 en0 = enemy.GetComponent<Enemy0>();
            en0.SetLine(enemyManager.GetActualLine());
            en0.SetEnemyManager(enemyManager);
        }
        enemyManager.SetTotalLineEnemies(total);
        enemyManager.UpdateActualLine();
    }

    void GenerateEnemies1(bool up)
    {
        //total enemies in line
        int total = 3;
        for(int i = 0; i < total; i++)
        {
            enemy = Instantiate(enemy1Prefab) as GameObject;
            enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);
            
            float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
            float enemyDistanceY = i * menuHeight * 0.2f;
            float positionY = Squares.totalSquaresY / 2f - menuHeight * 0.3f - enemyDistanceY + SpriteBounds.GetSpriteHeight(enemy);
            enemy.transform.position = new Vector2(positionX, positionY);

            enemy.GetComponent<Bounds>().Init(180f, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));

            Enemy en = enemy.GetComponent<Enemy>();
            en.SetLifes(1);
            en.SetScore(100);
            en.SetInformation(info);

            Enemy1 en1 = enemy.GetComponent<Enemy1>();
            float speedY = menuHeight / 4f;
            if (!up)
            {
                speedY *= -1;
            }
            en1.Init(-menuWidth / 4f, speedY, enemy.transform.position.y + menuHeight * 0.15f - SpriteBounds.GetSpriteWidth(enemy), 
                enemy.transform.position.y - menuHeight * 0.15f - SpriteBounds.GetSpriteWidth(enemy));
        }
    }

    void GenerateEnemies2(bool newUpgrade)
    {
        //total enemies in line
        int total = 2;
        for (int i = 0; i < total; i++)
        {
            if (newUpgrade)
            {
                enemy = Instantiate(enemy2RedPrefab) as GameObject;
            }
            else
            {
                enemy = Instantiate(enemy2Prefab) as GameObject;
            }
            enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);

            float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
            float enemyDistanceY = i * menuHeight * 0.3f;
            float positionY = Squares.totalSquaresY / 2f - menuHeight * 0.3f - enemyDistanceY + SpriteBounds.GetSpriteHeight(enemy) 
                -SpriteBounds.GetSpriteWidth(enemy);
            enemy.transform.position = new Vector2(positionX, positionY);

            enemy.GetComponent<Bounds>().Init(180f, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));

            Enemy en = enemy.GetComponent<Enemy>();
            en.SetLifes(1);
            en.SetScore(100);
            en.SetUpgrade(newUpgrade);
            en.SetInformation(info);

            enemy.GetComponent<Enemy2>().Init(-menuWidth / 3f, menuHeight / 10f, positionY);
        }
    }

    void GenerateEnemy3(bool up)
    {
        enemy = Instantiate(enemy3Prefab) as GameObject;
        enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);
        float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY;
        if (up)
        {
            positionY = Squares.totalSquaresY / 2f - menuHeight / 15f - SpriteBounds.GetSpriteHeight(enemy) / 5f;
        }
        else
        {
            positionY = Squares.totalSquaresY / 2f - 14f * menuHeight / 15f + SpriteBounds.GetSpriteHeight(enemy) / 2f;
        }
        enemy.transform.position = new Vector2(positionX, positionY);
        enemy.GetComponent<Bounds>().Init(180f, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));

        Enemy en = enemy.GetComponent<Enemy>();
        en.SetLifes(1);
        en.SetScore(100);
        en.SetInformation(info);

        enemy.GetComponent<ForwardMovement>().Init(menuWidth / 7f, 180f);

        enemy.GetComponent<ShootToShip>().SetShip(gradiusManager.GetActualShip().transform);

        AimToShip aim = enemy.GetComponent<AimToShip>();
        aim.SetShip(gradiusManager.GetActualShip().transform);
        aim.SetUp(up);
        if(total > 1)
            gradiusManager.UpdateActualShipIndex();
    }

    void GenerateEnemy4(bool newUpgrade)
    {
        if (newUpgrade)
        {
            enemy = Instantiate(enemy4RedPrefab) as GameObject;
        }
        else
        {
            enemy = Instantiate(enemy4Prefab) as GameObject;
        }
        enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);

        float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY = Squares.totalSquaresY / 2f - 9 * menuHeight / 10f;

        enemy.transform.position = new Vector2(positionX, positionY);
        
        enemy.GetComponent<Bounds>().Init(180f, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));

        Enemy en = enemy.GetComponent<Enemy>();
        en.SetLifes(1);
        en.SetScore(100);
        en.SetUpgrade(newUpgrade);
        en.SetInformation(info);

        ForwardMovementRB forward = enemy.GetComponent<ForwardMovementRB>();
        forward.Init();
        forward.SetSpeed(-menuWidth / 2f, 0f);
    }

    void GenerateEnemy5(bool newUpgrade, bool up)
    {

        if (newUpgrade)
        {
            if (up)
            {
                enemy = Instantiate(enemy5UpRedPrefab) as GameObject;
            }
            else
            {
                enemy = Instantiate(enemy5DownRedPrefab) as GameObject;
            }
        }
        else
        {
            if (up)
            {
                enemy = Instantiate(enemy5UpPrefab) as GameObject;
            }
            else
            {
                enemy = Instantiate(enemy5DownPrefab) as GameObject;
            }
        }
        enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);

        float positionX = -menuWidth / 2f - SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY;
        if (up)
        {
            positionY = Squares.totalSquaresY / 2f - menuHeight / 13f - SpriteBounds.GetSpriteHeight(enemy) / 2f;
            Enemy5Up en5 = enemy.GetComponent<Enemy5Up>();
            en5.SetSpeedX(menuWidth / 4f);
            en5.SetPausedSpeed(menuWidth / 7f);
            en5.SetShip(gradiusManager.GetActualShip());
        }
        else
        {
            positionY = Squares.totalSquaresY / 2f - 12f * menuHeight / 13f + SpriteBounds.GetSpriteHeight(enemy) / 2f;
            Enemy5 en5 = enemy.GetComponent<Enemy5>();
            en5.SetSpeedX(menuWidth / 4f);
            en5.SetSpeedY(Squares.totalSquaresY / 5f);
            en5.SetPausedSpeed(menuWidth / 7f);
            en5.SetFallSpeed(-Squares.totalSquaresY * 0.6f);
            en5.SetShip(gradiusManager.GetActualShip());
        }
        enemy.transform.position = new Vector2(positionX, positionY);
        enemy.GetComponent<Bounds>().Init(180f, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));
        Enemy en = enemy.GetComponent<Enemy>();
        en.SetLifes(1);
        en.SetScore(100);
        en.SetUpgrade(newUpgrade);
        en.SetInformation(info);

        ShootToShip shootToShip = enemy.GetComponent<ShootToShip>();
        shootToShip.SetShip(gradiusManager.GetActualShip().transform);
        shootToShip.SetTimeToShoot(0.5f);
        if (total > 1)
            gradiusManager.UpdateActualShipIndex();
    }

    void GenerateEnemy6(bool up)
    {
        
        enemy = Instantiate(enemy6Prefab) as GameObject;
        SpriteBounds.SetScaleSquare(enemy, menuWidth / 9f, menuHeight / 8f);

        float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY;
        if (up)
        {
            positionY = Squares.totalSquaresY / 2f - menuHeight / 15f - SpriteBounds.GetSpriteHeight(enemy) / 3f;
        }
        else
        {
            positionY = Squares.totalSquaresY / 2f - 14f * menuHeight / 15f + SpriteBounds.GetSpriteHeight(enemy) / 2f;
        }
        enemy.transform.position = new Vector2(positionX, positionY);

        enemy.GetComponent<Bounds>().Init(180f, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));
        Enemy en = enemy.GetComponent<Enemy>();
        en.SetLifes(3);
        en.SetScore(300);
        en.SetInformation(info);

        ForwardMovementRB forward = enemy.GetComponent<ForwardMovementRB>();
        forward.Init();
        forward.SetSpeed(-menuWidth / 7f, 0f);

        Enemy6 en6 = enemy.GetComponent<Enemy6>();
        en6.SetTimeToShoot(1f);
        en6.SetUp(up);
        en6.SetEnemyGenerator(this);
    }
    //objHeight, posY and posX are the height, position y and position x of the enemy6
    public void GenerateEnemy7(bool up, float posX, float posY, float objHeight)
    {
        enemy = Instantiate(enemy7Prefab) as GameObject;
        SpriteBounds.SetScaleSquare(enemy, menuWidth / 18, menuHeight / 16f);

        Enemy7 en7 = enemy.GetComponent<Enemy7>();
        en7.SetSpeedX(-menuWidth / 7f);
        en7.SetShip(gradiusManager.GetActualShip().transform);

        float angle = 180;
        float positionY;
        if (up)
        {
            angle += 45f;
            positionY = posY - objHeight / 2f - SpriteBounds.GetSpriteHeight(enemy) / 2f;
            en7.SetSpeedY(-menuHeight / 3f);
        }
        else
        {
            angle -= 45f;
            positionY = posY + objHeight / 2f + SpriteBounds.GetSpriteHeight(enemy) / 2f;
            en7.SetSpeedY(menuHeight / 3f);
        }
        enemy.GetComponent<Bounds>().Init(angle, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));
        enemy.transform.position = new Vector2(posX, positionY);

        Enemy en = enemy.GetComponent<Enemy>();
        en.SetLifes(1);
        en.SetScore(100);
        en.SetInformation(info);

        ShootToShip shootToShip = enemy.GetComponent<ShootToShip>();
        shootToShip.SetShip(gradiusManager.GetActualShip().transform);
        shootToShip.SetTimeToShoot(1.0f);
        if (total > 1)
            gradiusManager.UpdateActualShipIndex();
    }

    public void GenerateEnemies8()
    {
        //position 0 -> left, 1->right
        int total = 2;
        for (int i = 0; i < total; i++)
        {
            enemy = Instantiate(enemy8Prefab) as GameObject;
            SpriteBounds.SetScaleSquare(enemy, menuWidth / 18, menuHeight / 18f);

            float y = menuHeight * 6.2f / 8.9f;
            float posY = Squares.totalSquaresY / 2f - y;
            float x;
            float posX;
            if(i == 0)
            {
                x = menuWidth * 3.6f / 15f;
            }
            else
            {
                x = menuWidth * 12.2f / 15f;
            }
            posX = -menuWidth / 2f + x;
            
            enemy.transform.position = new Vector2(posX, posY);


            float speedX = Random.Range(menuWidth / 6f, menuWidth / 2f);
            float speedY = Random.Range(menuHeight * 0.8f, menuHeight * 1.2f);
            if(Random.value < 0.5)
            {
                speedX *= -1;
            }
            Enemy8 en8 = enemy.GetComponent<Enemy8>();
            en8.SetSpeedX(speedX);
            en8.SetForceY(speedY);

            float angle = 270f;
            if (speedX < 0)
            {
                angle -= 45f;
            }
            else
            {
                angle += 45f;
            }
            enemy.GetComponent<Bounds>().Init(angle, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));
            
            Enemy en = enemy.GetComponent<Enemy>();
            en.SetLifes(1);
            en.SetScore(100);
            en.SetInformation(info);
        }
    }

    public void GenerateEnemy9()
    {
        enemy = Instantiate(enemy9Prefab) as GameObject;
        SpriteBounds.SetScaleSquare(enemy, menuWidth / 6, menuHeight / 4f);

        float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY = Squares.totalSquaresY / 2f - menuHeight / 2f;
        enemy.transform.position = new Vector2(positionX, positionY);

        Enemy en = enemy.GetComponent<Enemy>();
        en.SetLifes(100);
        en.SetScore(1000);
        en.SetInformation(info);

        Enemy9 en9 = enemy.GetComponent<Enemy9>();
        en9.SetSpeedX(-menuWidth / 3f);
        en9.SetSpeedY(menuHeight / 2.66f);
        en9.SetTimeToShoot(1.0f);
        en9.SetLevel(level);
        en9.SetTotal(total);
        if (total < 2)
        {
            en9.SetShip(gradiusManager.GetActualShip().transform);
        }
        else
        {
            en9.SetLimitYUp(Squares.totalSquaresY / 2f - SpriteBounds.GetSpriteHeight(enemy) / 2f);
            en9.SetLimitYDown(Squares.totalSquaresY / 2f - menuHeight + SpriteBounds.GetSpriteHeight(enemy) / 2f);
        }
    }
}
