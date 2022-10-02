using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemy9Prefab;
    [SerializeField] private GradiusManager gradiusManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private LevelInfo level;
    [SerializeField] private GameMenuInformation info;
    [SerializeField] private ObjectPool enemyBulletPool;
    [SerializeField] private ObjectPool bossEnemyBulletPool;
    private GameObject enemy9;
    /*
     Enemies: 0,1,2,2R,3,4,4R,5D,5DR,5U,5UR,6,7,8
     */
    [SerializeField] private ObjectPool[] enemyPools;
    //auxiliar variables to create enemies
    BoundsPoolObject bound;

    private GameObject enemy;

    private float menuWidth;
    private float menuHeight;
    int total;

    //auxiliar variables to generate new bullets
    ShootToShip shoot;
    ForwardMovementRB forward;
    EnemyInfo e;
    public void SetActiveEnemy9(bool value)
    {
        enemy9.SetActive(value);
    }
    private void Start()
    {
        total = PlayerVariables.Instance.GetPlayers();
        menuWidth = SquaresResolution.TotalSquaresX;
        menuHeight = SquaresResolution.TotalSquaresY * 0.89f;
        enemy9 = Instantiate(enemy9Prefab) as GameObject;
        SpriteBounds.SetScaleSquare(enemy9, menuWidth / 6, menuHeight / 4f);
        
        BossBehaviour en9 = enemy9.GetComponent<BossBehaviour>();
        en9.SetSpeedX(-menuWidth / 3f);
        en9.SetSpeedY(menuHeight / 2.66f);
        en9.SetTimeToShoot(1.0f);
        en9.SetLevel(level);
        en9.SetLimitYUp(SquaresResolution.TotalSquaresY / 2f - SpriteBounds.GetSpriteHeight(enemy9) / 2f);
        en9.SetLimitYDown(SquaresResolution.TotalSquaresY / 2f - menuHeight + SpriteBounds.GetSpriteHeight(enemy9) / 2f);
        en9.SetBulletPool(bossEnemyBulletPool);
        enemy9.SetActive(false);
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
    void SetEnemyBoundsPool(float angle, int indexPool)
    {
        bound = enemy.GetComponent<BoundsPoolObject>();
        bound.Init(angle, SpriteBounds.GetSpriteWidth(enemy), SpriteBounds.GetSpriteHeight(enemy));
        if (!bound.HasObjectPool())
        {
            bound.SetObjectPool(enemyPools[indexPool]);
        }
    }
    void SetEnemyInformation(int lifes, int score, bool upg)
    {
        e = enemy.GetComponent<EnemyInfo>();
        e.SetLifes(lifes);
        e.SetScore(score);
        e.SetUpgrade(upg);
        e.SetDead(false);
        if (!e.HasInfo())
        {
            e.SetInformation(info);
        }
    }
    void SetShootToShipInfo(float timer)
    {
        shoot = enemy.GetComponent<ShootToShip>();
        shoot.shoots = 0;
        shoot.SetShip(gradiusManager.GetActualShip().transform);
        shoot.SetTimer(timer);
        if (!shoot.HasBulletPool())
        {
            shoot.SetBulletPool(enemyBulletPool);
        }
    }
    void GenerateEnemies0(bool up)
    {
        //total enemies in line
        int total = 4;
        for (int i = 0; i < total; i++)
        {
            enemy = enemyPools[0].GetObjectFromPool();
            enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);
            float positionY;
            float positionX;
            if (up)
            {
                positionY = SquaresResolution.TotalSquaresY / 2f - menuHeight * 0.15f;
            }
            else
            {
                positionY = SquaresResolution.TotalSquaresY / 2f - menuHeight * 0.85f;
            }

            BoxCollider2D collider = enemy.GetComponent<BoxCollider2D>();
            positionX = menuWidth / 2f + collider.size.x * (1.5f * i + 0.5f);
            enemy.transform.position = new Vector2(positionX, positionY);

            SetEnemyBoundsPool(180f, 0);
            SetEnemyInformation(1, 100, false);

            forward = enemy.GetComponent<ForwardMovementRB>();
            forward.Init();
            forward.SetSpeed(-menuWidth * 0.4166f, 0f);

            Enemy0Information en0 = enemy.GetComponent<Enemy0Information>();
            en0.SetLine(enemyManager.GetActualLine());
            if (!en0.HasEnemyManager())
            {
                en0.SetEnemyManager(enemyManager);
            }
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
            enemy = enemyPools[1].GetObjectFromPool();
            enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);
            float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
            float enemyDistanceY = i * menuHeight * 0.2f;
            float positionY = SquaresResolution.TotalSquaresY / 2f - menuHeight * 0.3f - enemyDistanceY + SpriteBounds.GetSpriteHeight(enemy);
            enemy.transform.position = new Vector2(positionX, positionY);
            SetEnemyBoundsPool(180f, 1);
            SetEnemyInformation(1, 100, false);
            UpAndDownMovement en1 = enemy.GetComponent<UpAndDownMovement>();
            float speedY = menuHeight / 4f;
            if (!up)
            {
                speedY *= -1;
            }
            en1.InitRigidBody();
            en1.Init(-menuWidth / 4f, speedY, enemy.transform.position.y + menuHeight * 0.15f - SpriteBounds.GetSpriteWidth(enemy), 
                enemy.transform.position.y - menuHeight * 0.15f - SpriteBounds.GetSpriteWidth(enemy));
        }
    }

    void GenerateEnemies2(bool newUpgrade)
    {
        int indexPool;
        //total enemies in line
        int total = 2;
        for (int i = 0; i < total; i++)
        {
            if (newUpgrade)
            {
                indexPool = 3;
            }
            else
            {
                indexPool = 2;
            }
            enemy = enemyPools[indexPool].GetObjectFromPool();
            enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);
            float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
            float enemyDistanceY = i * menuHeight * 0.3f;
            float positionY = SquaresResolution.TotalSquaresY / 2f - menuHeight * 0.3f - enemyDistanceY + SpriteBounds.GetSpriteHeight(enemy) 
                -SpriteBounds.GetSpriteWidth(enemy);
            enemy.transform.position = new Vector2(positionX, positionY);
            SetEnemyBoundsPool(180f, indexPool);
            SetEnemyInformation(1, 100, newUpgrade);
            enemy.GetComponent<HarmonicWaveMovement>().Init(-menuWidth / 3f, menuHeight / 10f, positionY);
        }
    }

    void GenerateEnemy3(bool up)
    {
        enemy = enemyPools[4].GetObjectFromPool();
        enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);
        float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY;
        if (up)
        {
            positionY = SquaresResolution.TotalSquaresY / 2f - menuHeight / 15f - SpriteBounds.GetSpriteHeight(enemy) / 5f;
        }
        else
        {
            positionY = SquaresResolution.TotalSquaresY / 2f - 14f * menuHeight / 15f + SpriteBounds.GetSpriteHeight(enemy) / 2f;
        }
        enemy.transform.position = new Vector2(positionX, positionY);
        SetEnemyBoundsPool(180f, 4);
        SetEnemyInformation(1, 100, false);
        enemy.GetComponent<ForwardMovement>().Init(menuWidth / 7f, 180f);
        SetShootToShipInfo(0.5f);
        AimToShip aim = enemy.GetComponent<AimToShip>();
        aim.SetShip(gradiusManager.GetActualShip().transform);
        aim.SetUp(up);
        if(total > 1)
            gradiusManager.UpdateActualShipIndex();
    }

    void GenerateEnemy4(bool newUpgrade)
    {
        int indexPool;
        if (newUpgrade)
        {
            indexPool = 6;
        }
        else
        {
            indexPool = 5;
        }
        enemy = enemyPools[indexPool].GetObjectFromPool();
        enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);

        float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY = SquaresResolution.TotalSquaresY / 2f - 9 * menuHeight / 10f;

        enemy.transform.position = new Vector2(positionX, positionY);

        SetEnemyBoundsPool(180f, indexPool);
        SetEnemyInformation(1, 100, newUpgrade);

        enemy.GetComponent<JumpOnLayer>().SetJumpSpeed(SquaresResolution.TotalSquaresY);

        forward = enemy.GetComponent<ForwardMovementRB>();
        forward.Init();
        forward.SetSpeed(-menuWidth / 2f, 0f);
    }

    void GenerateEnemy5(bool newUpgrade, bool up)
    {
        int indexPool;
        if (newUpgrade)
        {
            if (up)
            {
                indexPool = 10;
            }
            else
            {
                indexPool = 8;
            }
        }
        else
        {
            if (up)
            {
                indexPool = 9;
            }
            else
            {
                indexPool = 7;
            }
        }
        enemy = enemyPools[indexPool].GetObjectFromPool();
        enemy.transform.localScale = new Vector2(menuWidth / 18f, menuHeight / 12f);

        float positionX = -menuWidth / 2f - SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY;
        if (up)
        {
            positionY = SquaresResolution.TotalSquaresY / 2f - menuHeight / 13f - SpriteBounds.GetSpriteHeight(enemy) / 2f;
            Enemy5FloorBehaviour en5 = enemy.GetComponent<Enemy5FloorBehaviour>();
            en5.SetSpeedX(menuWidth / 4f);
            en5.SetPausedSpeed(menuWidth / 7f);
            en5.SetShip(gradiusManager.GetActualShip());
            en5.SetPaused(false);
            en5.SetShoots(0);
            enemy.transform.rotation = new Quaternion(180f, 0f, 0f, 0f);
        }
        else
        {
            positionY = SquaresResolution.TotalSquaresY / 2f - 12f * menuHeight / 13f + SpriteBounds.GetSpriteHeight(enemy) / 2f;
            Enemy5FloorMountainBehaviour en5 = enemy.GetComponent<Enemy5FloorMountainBehaviour>();
            en5.SetSpeedX(menuWidth / 4f);
            en5.SetSpeedY(SquaresResolution.TotalSquaresY / 5f);
            en5.SetPausedSpeed(menuWidth / 7f);
            en5.SetFallSpeed(-SquaresResolution.TotalSquaresY * 0.6f);
            en5.SetShip(gradiusManager.GetActualShip());
            en5.SetPaused(false);
            en5.SetShoots(0);
            enemy.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        enemy.transform.position = new Vector2(positionX, positionY);
        SetEnemyBoundsPool(180f, indexPool);
        SetEnemyInformation(1, 100, newUpgrade);

        SetShootToShipInfo(0.5f);
        shoot.SetTimeToShoot(0.5f);
        shoot.enabled = false;

        if (total > 1)
            gradiusManager.UpdateActualShipIndex();
    }

    void GenerateEnemy6(bool up)
    {
        enemy = enemyPools[11].GetObjectFromPool();
        float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY;
        if (up)
        {
            positionY = SquaresResolution.TotalSquaresY / 2f - menuHeight / 15f - SpriteBounds.GetSpriteHeight(enemy) / 3f;
        }
        else
        {
            positionY = SquaresResolution.TotalSquaresY / 2f - 14f * menuHeight / 15f + SpriteBounds.GetSpriteHeight(enemy) / 2f;
        }
        enemy.transform.position = new Vector2(positionX, positionY);

        SetEnemyBoundsPool(180f, 11);
        SetEnemyInformation(3, 300, false);

        forward = enemy.GetComponent<ForwardMovementRB>();
        forward.Init();
        forward.SetSpeed(-menuWidth / 7f, 0f);

        EnemyMachineGeneratorBehaviour en6 = enemy.GetComponent<EnemyMachineGeneratorBehaviour>();
        en6.SetTimeToShoot(1f);
        en6.SetTimer(1f);
        en6.SetUp(up);
        if (!en6.HasEnemyGenerator())
        {
            en6.SetEnemyGenerator(this);
        }
    }
    //objHeight, posY and posX are the height, position y and position x of the enemy6
    public void GenerateEnemy7(bool up, float posX, float posY, float objHeight)
    {
        enemy = enemyPools[12].GetObjectFromPool();
        EnemyFromMachineBehaviour en7 = enemy.GetComponent<EnemyFromMachineBehaviour>();
        en7.InitRigidBody();
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
        en7.InitVelocity();
        en7.SetPaused(false);
        enemy.transform.position = new Vector2(posX, positionY);

        SetEnemyBoundsPool(angle, 12);
        SetEnemyInformation(1, 100, false);

        SetShootToShipInfo(1f);
        shoot.SetTimeToShoot(1.0f);
        shoot.enabled = false;

        if (total > 1)
            gradiusManager.UpdateActualShipIndex();
    }

    public void GenerateEnemies8()
    {
        //position 0 -> left, 1->right
        int total = 2;
        for (int i = 0; i < total; i++)
        {
            enemy = enemyPools[13].GetObjectFromPool();

            float y = menuHeight * 6.2f / 8.9f;
            float posY = SquaresResolution.TotalSquaresY / 2f - y;
            float x = 5f * SquaresResolution.TotalSquaresX / 17.8f;
            float posX;
            if(i == 0)
            {
                posX = -x;
            }
            else
            {
                posX = x;
            }
            
            enemy.transform.position = new Vector2(posX, posY);


            float speedX = Random.Range(menuWidth / 6f, menuWidth / 2f);
            float speedY = Random.Range(menuHeight * 0.8f, menuHeight * 1.2f);
            if(Random.value < 0.5)
            {
                speedX *= -1;
            }
            forward = enemy.GetComponent<ForwardMovementRB>();
            forward.Init();
            forward.SetSpeed(speedX, speedY);

            float angle = 270f;
            if (speedX < 0)
            {
                angle -= 45f;
            }
            else
            {
                angle += 45f;
            }
            SetEnemyBoundsPool(angle, 13);
            SetEnemyInformation(1, 100, false);
        }
    }

    public void GenerateEnemy9()
    {
        enemy = enemy9;
        enemy.SetActive(true);
        float positionX = menuWidth / 2f + SpriteBounds.GetSpriteWidth(enemy) / 2f;
        float positionY = SquaresResolution.TotalSquaresY / 2f - menuHeight / 2f;
        enemy.transform.position = new Vector2(positionX, positionY);
        SetEnemyInformation(100, 1000, false);

        BossBehaviour en9 = enemy.GetComponent<BossBehaviour>();
        en9.SetTimer(0f);
        en9.SetStop(false);

        en9.SetTotal(total);
        if (total < 2)
        {
            en9.SetShip(gradiusManager.GetActualShip().transform);
        }
    }
}
