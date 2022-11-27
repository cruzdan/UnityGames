using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Ship : MonoBehaviour
{
    private float speedX;
    private float speedY;
    private float firstX;
    private float firstY;
    private float endX;
    private float endY;
    [SerializeField] private int shipIndex = 0;
    [SerializeField] private GameMenuInformation info;
    private bool dead = false;


    //Upgrades
    private Shoot sh;
    private bool missileUpgrade = false;
    private bool doubleUpgrade = false;
    private bool laserUpgrade = false;
    private float incrementSpeedX;
    private float incrementSpeedY;

    [SerializeField] private UpgradeRectsManager upgradeRects;

    //invincible
    private bool invincible = false;
    private float timerInvincible = 0f;
    [SerializeField] private float timeInvincible = 3f;
    bool visible = true;

    private float shootTimer = 0f;

    [Header("Keys")]
    [SerializeField] private KeyCode rightKey;
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode upKey;
    [SerializeField] private KeyCode downKey;
    [SerializeField] private KeyCode shootKey;

    [Header("Option Upgrade")]
    [SerializeField] private GameObject optionPrefab;
    private GameObject[] options;
    private int totalOptions = 0;

    [Header("Shield Upgrade")]
    [SerializeField] private GameObject shieldPrefab;
    private GameObject shield;

    [Header("Animation")]
    private Animator animator;

    [Header("Animation")]
    [SerializeField] private GameObject missilePrefab;
    private GameObject missile;

    [SerializeField] private EnemyManager enemyManager;
    

    //variable to know when the ship is moving
    //Vector2 movement = new Vector2();
    float moving;
    //variable to control the ship next position
    float pos;
    //variable to control the ship animation, speed on y axis: 1 = up, -1 = down, 0 = no speed
    float vert = 0.0f;

    //auxiliar variable to appear an option prefab
    Option op;
    PlayerInput playerInput;
    void Start()
    {
        sh = GetComponent<Shoot>();
        sh.SetEnemyManager(enemyManager);
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        //this width is the ship sprite width
        float width = SpriteBounds.GetSpriteWidth(gameObject);
        firstX = -SquaresResolution.TotalSquaresX / 2.0f + width / 2.0f;
        endX = -firstX;
        firstY = SquaresResolution.TotalSquaresY / 2.0f - transform.localScale.y / 2.0f;
        endY = -(0.78f * SquaresResolution.TotalSquaresY / 2.0f - transform.localScale.y / 2.0f);

        options = new GameObject[2];
        InitOption(0);
        InitOption(1);

        shield = Instantiate(shieldPrefab) as GameObject;
        Shield shi = shield.GetComponent<Shield>();
        shi.SetUpgradeRect(upgradeRects);
        shi.SetEnemyManager(enemyManager);
        shi.SetShipIndex(shipIndex);
        shield.SetActive(false);

        missile = Instantiate(missilePrefab) as GameObject;
        InitMissile(missile);
        InitMissile(options[0].GetComponent<Option>().missile);
        InitMissile(options[1].GetComponent<Option>().missile);

        timerInvincible = timeInvincible;
    }
    public UpgradeRectsManager GetUpgradeRectsManager() { return upgradeRects; }
    public void SetInputDevice(InputDevice inputDevice)
    {
        // Pair the gamepad or joystick to a user.
        var user = InputUser.PerformPairingWithDevice(inputDevice);
        user.AssociateActionsWithUser(GetComponent<PlayerInput>().currentActionMap);
    }
    void InitMissile(GameObject missileObject)
    {
        Missile mis = missileObject.GetComponent<Missile>();
        mis.SetEnemyManager(enemyManager);
        CollisionBulletToEnemy collision = missileObject.GetComponent<CollisionBulletToEnemy>();
        collision.SetEnemyManager(enemyManager);
        collision.SetDamage(1);
        collision.SetShipIndex(shipIndex);
        missileObject.SetActive(false);
    }
    void InitOption(int index)
    {
        options[index] = Instantiate(optionPrefab) as GameObject;
        options[index].GetComponent<Option>().Init(missilePrefab);
        SpriteBounds.SetScaleSquare(options[index], SpriteBounds.GetSpriteWidth(gameObject) / 2f,
            SpriteBounds.GetSpriteHeight(gameObject));
        options[index].SetActive(false);
    }
    void Update()
    {
        if (playerInput.actions["Right"].IsPressed() || Input.GetKey(rightKey) )
        {
            MoveToRight();
        }
        else if (playerInput.actions["Left"].IsPressed() || Input.GetKey(leftKey))
        {
            MoveToLeft();
        }
        
        vert = 0.0f;
        
        if (playerInput.actions["Up"].IsPressed() || Input.GetKey(upKey))
        {
            MoveToUp();
        }
        else if (playerInput.actions["Down"].IsPressed() || Input.GetKey(downKey))
        {
            MoveToDown();
        }
        animator.SetFloat("Vertical", vert);

        //shoot

        if (playerInput.actions["Shoot"].IsPressed() || Input.GetKey(shootKey))
        {
            Shoot();
        }
        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }

        if (invincible)
        {
            timerInvincible -= Time.deltaTime;
            int k = (int)(timerInvincible * 10) % 5;
            if (k < 2.5f)
            {
                if (visible)
                {
                    visible = false;
                    GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                }
            }
            else
            {
                if (!visible)
                {
                    visible = true;
                    GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                }
            }
            if (timerInvincible < 0)
            {
                invincible = false;
                timerInvincible = timeInvincible;
                visible = true;
                GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            }
        }
    }
    public void Shoot()
    {
        if (shootTimer <= 0)
        {
            ShootBullet();
            shootTimer = 0.25f;
        }
        if (missileUpgrade)
        {
            if (!missile.activeSelf)
            {
                sh.ShootMissile(missile, transform.position.x, transform.position.y, transform.localScale.x, shipIndex);
            }
            
            for (int i = 0; i < totalOptions; i++)
            {
                if (!options[i].GetComponent<Option>().missile.activeSelf)
                {
                    sh.ShootMissile(options[i].GetComponent<Option>().missile, options[i].transform.position.x, options[i].transform.position.y,
                        options[i].transform.localScale.x, shipIndex);
                }
            }
        }
    }
    void MoveToRight()
    {
        pos = transform.position.x + speedX * Time.deltaTime;
        if (pos < endX)
        {
            transform.position = new Vector2(pos, transform.position.y);
            MoveShield();
        }
        MoveOptions();
    }
    void MoveToLeft()
    {
        pos = transform.position.x - speedX * Time.deltaTime;
        if (pos > firstX)
        {
            transform.position = new Vector2(pos, transform.position.y);
            MoveShield();
        }
        MoveOptions();
    }
    void MoveToUp()
    {
        pos = transform.position.y + speedY * Time.deltaTime;
        if (pos < firstY)
        {
            transform.position = new Vector2(transform.position.x, pos);
            MoveShield();
        }
        vert = 1.0f;
        MoveOptions();
    }
    void MoveToDown()
    {
        pos = transform.position.y - speedY * Time.deltaTime;
        if (pos > endY)
        {
            transform.position = new Vector2(transform.position.x, pos);
            MoveShield();
        }
        vert = -1.0f;
        MoveOptions();
    }
    void ShootBullet()
    {
        if (doubleUpgrade || laserUpgrade)
        {
            if (doubleUpgrade)
            {
                sh.ShootForwardBullet(SquaresResolution.TotalSquaresInclined / 1.8f, transform.position.x, 
                    transform.position.y, transform.localScale.x, shipIndex);
                sh.ShootInclinedBullet(SquaresResolution.TotalSquaresInclined / 1.8f, transform.position.x, 
                    transform.position.y, transform.localScale.x, shipIndex);
                
				for (int i = 0; i < totalOptions; i++)
				{
					sh.ShootForwardBullet(SquaresResolution.TotalSquaresInclined / 1.8f, options[i].transform.position.x, 
                        options[i].transform.position.y, options[i].transform.localScale.x, shipIndex);
					sh.ShootInclinedBullet(SquaresResolution.TotalSquaresInclined / 1.8f, options[i].transform.position.x,
                        options[i].transform.position.y, options[i].transform.localScale.x, shipIndex);
				}
            }
            else
            {
                sh.ShootLaserBullet(SquaresResolution.TotalSquaresInclined / 1.2f, transform.position.x, 
                    transform.position.y, transform.localScale.x, shipIndex);
				for (int i = 0; i < totalOptions; i++)
				{
                    sh.ShootLaserBullet(SquaresResolution.TotalSquaresInclined / 1.2f, options[i].transform.position.x,
                        options[i].transform.position.y, options[i].transform.localScale.x, shipIndex);
				}
            }
        }
        else
        {
            sh.ShootForwardBullet(SquaresResolution.TotalSquaresInclined / 1.8f, transform.position.x, 
                transform.position.y, transform.localScale.x, shipIndex);
			for (int i = 0; i < totalOptions; i++)
			{
                sh.ShootForwardBullet(SquaresResolution.TotalSquaresInclined / 1.8f, options[i].transform.position.x,
                        options[i].transform.position.y, options[i].transform.localScale.x, shipIndex);
			}
        }
    }

    void CreateOptionToShip()
    {
        options[0].SetActive(true);
        op = options[0].GetComponent<Option>();
        for (int i = 10; i >= 1; i--)
        {
            op.SetRoute(transform.position.x, transform.position.y - (speedY * Time.deltaTime * i), 10 - i);
        }
        options[0].transform.position = new Vector2(op.GetRouteX(0), op.GetRouteY(0));
        op.RestartIndex();
        op.missile.SetActive(false);
    }

    void CreateOptionToOption()
    {
        options[1].SetActive(true);
        op = options[1].GetComponent<Option>();
        for (int i = 10; i >= 1; i--)
        {
            op.SetRoute(options[0].transform.position.x, options[0].transform.position.y - (speedY * Time.deltaTime * i), 10 - i);
        }
        options[1].transform.position = new Vector2(op.GetRouteX(0), op.GetRouteY(0));
        op.RestartIndex();
        op.missile.SetActive(false);
    }

    void MoveOptions()
    {
         for(int i = 0; i < totalOptions; i++)
        {
            if(i == 0)
                options[i].GetComponent<Option>().Move(transform.position.x, transform.position.y);
            else
                options[i].GetComponent<Option>().Move(options[0].transform.position.x, options[0].transform.position.y);
        }
    }

    void MoveShield()
    {
        if (shield.activeSelf)
        {
            shield.transform.position = new Vector2(
                transform.position.x + SpriteBounds.GetSpriteWidth(gameObject) / 2.0f + 
                SpriteBounds.GetSpriteWidth(shield) / 2.0f,
                transform.position.y);
        }
    }
    public void IncrementSpeed()
    {
        speedX += incrementSpeedX;
        speedY += incrementSpeedY;
    }

    public void SetMissileUpgrade(bool value)
    {
        missileUpgrade = value;
    }

    public void SetDoubleUpgrade(bool value)
    {
        if (doubleUpgrade)
            doubleUpgrade = false;
        laserUpgrade = true;
    }
    public void SetLaserUpgrade(bool value)
    {
        if (laserUpgrade)
            laserUpgrade = false;
        doubleUpgrade = true;
    }

    public void AddOption()
    {
        totalOptions++;
        if(totalOptions == 1)
        {
            CreateOptionToShip();
        }
        else
        {
            CreateOptionToOption();
        }
    }
    public void CreateShield()
    {
        shield.SetActive(true);
        shield.GetComponent<Shield>().SetLifes(10);
        shield.GetComponent<Shield>().SetDead(false);
        MoveShield();
    }

    public void SetEnemyManager(EnemyManager e)
    {
        enemyManager = e;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            //enemies layer
            case 8:
            case 17:
            case 18:
            case 19:
            //Map
            case 13:
            case 16:
                if (!invincible)
                {
                    invincible = true;
                    info.DecrementLifes(shipIndex);
                    
                }
                break;
            case 11:
                //upgrade layer
                if (!collision.gameObject.GetComponent<UpgradeDeadInfo>().dead)
                {
                    collision.gameObject.GetComponent<UpgradeDeadInfo>().dead = true;
                    upgradeRects.AddUpgrade();
                    collision.gameObject.GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(collision.gameObject);
                }
                break;
            //EnemyBullets
            case 7:
                if (!invincible)
                {
                    invincible = true;
                    info.DecrementLifes(shipIndex);
                    collision.gameObject.GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(collision.gameObject);
                }
                break;
        }
    }

    public void RestartOptions()
    {
        for(int i = 0; i < totalOptions; i++)
        {
            options[i].GetComponent<Option>().missile.SetActive(false);
            options[i].SetActive(false);
            
        }
        totalOptions = 0;
    }
    public void RestartShield()
    {
        shield.SetActive(false);
    }

    public void RestartUpgrades()
    {
        missileUpgrade = false;
        doubleUpgrade = false;
        laserUpgrade = false;
        missile.SetActive(false);
    }
    //0 -> right, 1 -> left, 2 -> up, 3-> down, 4 -> shoot
    public void SetKey(int indexKey, KeyCode key)
    {
        switch (indexKey)
        {
            case 0:
                upKey = key;
                break;
            case 1:
                downKey = key;
                break;
            case 2:
                rightKey = key;
                break;
            case 3:
                leftKey = key;
                break;
            case 4:
                shootKey = key;
                break;
        }
    }
    public void SetShipIndex(int index) { shipIndex = index; }
    public int GetShipIndex() { return shipIndex; }
    public void SetInformation(GameMenuInformation i) { info = i; }
    public void SetSpeedX(float speed) { speedX = speed; }
    public void SetSpeedY(float speed) { speedY = speed; }
    public void SetIncrementSpeedX(float speed) { incrementSpeedX = speed; }
    public void SetIncrementSpeedY(float speed) { incrementSpeedY = speed; }
    public void SetTimeInvincible(float time) { timeInvincible = time; }
    public void SetInvincible(bool value) { invincible = value; }
    public void SetTimerInvincible(float value) { timerInvincible = value; }
    public void SetVisible(bool value) { visible = value; }
    public float GetTimeInvincible() { return timeInvincible; }
    public void SetRightKey(KeyCode key) { rightKey = key; }
    public void SetLeftKey(KeyCode key) { leftKey = key; }
    public void SetUpKey(KeyCode key) { upKey = key; }
    public void SetDownKey(KeyCode key) { downKey = key; }
    public void SetShootKey(KeyCode key) { shootKey = key; }
    public void SetDead(bool value) { dead = value; }
    public void SetUpgradeRect(UpgradeRectsManager upgrade) { upgradeRects = upgrade; }
    public bool GetDead() { return dead; }
    public float GetIncrementSpeedX() { return incrementSpeedX; }
    public float GetIncrementSpeedY() { return incrementSpeedY; }
    public void SetBulletTimer(float time) { shootTimer = time; }
    public KeyCode GetUpKey() { return upKey; }
    public KeyCode GetDownKey() { return downKey; }
    public KeyCode GetRightKey() { return rightKey; }
    public KeyCode GetLeftKey() { return leftKey; }
    public KeyCode GetShootKey() { return shootKey; }
    public KeyCode GetSelectKey() { return upgradeRects.GetSelectKey(); }
    public UpgradeRectsManager GetUpgradeRects() { return upgradeRects; }
}