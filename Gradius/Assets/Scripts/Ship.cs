using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private float speedX;
    private float speedY;
    private float firstX;
    private float firstY;
    private float endX;
    private float endY;
    [SerializeField] private int shipIndex = 0;
    [SerializeField] private Information info;
    private bool dead = false;


    //Upgrades
    private Shoot sh;
    private bool missileUpgrade = false;
    private bool doubleUpgrade = false;
    private bool laserUpgrade = false;
    private float incrementSpeedX;
    private float incrementSpeedY;
    private bool hasMissile = false;

    [SerializeField] private UpgradeRects upgradeRects;

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

    [SerializeField] private EnemyManager enemyManager;

    public void SetUpgradeRect(UpgradeRects upgrade) { upgradeRects = upgrade; }
    public UpgradeRects GetUpgradeRects() { return upgradeRects; }
    // Start is called before the first frame update
    void Start()
    {
        sh = GetComponent<Shoot>();
        sh.SetEnemyManager(enemyManager);
        animator = GetComponent<Animator>();

        //this width is the ship sprite width
        float width = SpriteBounds.GetSpriteWidth(gameObject);
        firstX = -Squares.totalSquaresX / 2.0f + width / 2.0f;
        endX = -firstX;
        firstY = Squares.totalSquaresY / 2.0f - transform.localScale.y / 2.0f;
        endY = -(0.78f * Squares.totalSquaresY / 2.0f - transform.localScale.y / 2.0f);

        options = new GameObject[2];
        timerInvincible = timeInvincible;
    }

    // Update is called once per frame
    void Update()
    {
        float pos;
        //speed on y axis: 1 = up, -1 = down, 0 = no speed
        float vert = 0.0f;
        if (Input.GetKey(rightKey))
        {
            pos = transform.position.x + speedX * Time.deltaTime;
            if(pos < endX)
            {
                transform.position = new Vector2(pos, transform.position.y);
                MoveShield();
            }
            MoveOptions();
        }
        if (Input.GetKey(leftKey))
        {
            pos = transform.position.x - speedX * Time.deltaTime;
            if (pos > firstX)
            {
                transform.position = new Vector2(pos, transform.position.y);
                MoveShield();
            }
            MoveOptions();
        }
        if (Input.GetKey(upKey))
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
        if (Input.GetKey(downKey))
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
        animator.SetFloat("Vertical", vert);

        //shoot
        if (Input.GetKey(shootKey))
        {
            if (shootTimer <= 0)
            {
                ShootBullet();
                shootTimer = 0.25f;
            }
            if (missileUpgrade)
            {
                if (!hasMissile)
                {
                    sh.ShootMissile(this, 0, transform.position.x, transform.position.y, transform.localScale.x, shipIndex);
                    hasMissile = true;
                    
                }
                for(int i = 0; i  < totalOptions; i++)
                {
                    if (!options[i].GetComponent<Option>().hasMissile)
                    {
                        sh.ShootMissile(this, i + 1, options[i].transform.position.x, options[i].transform.position.y,
                            options[i].transform.localScale.x, shipIndex);
                        options[i].GetComponent<Option>().hasMissile = true;
                    }
                }
            }
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

    void ShootBullet()
    {
        if (doubleUpgrade || laserUpgrade)
        {
            if (doubleUpgrade)
            {
                sh.ShootForwardBullet(Squares.totalSquaresInclined / 1.8f, transform.position.x, transform.position.y, transform.localScale.x, shipIndex);
                sh.ShootInclinedBullet(Squares.totalSquaresInclined / 1.8f, transform.position.x, transform.position.y, transform.localScale.x, shipIndex);
                
				for (int i = 0; i < totalOptions; i++)
				{
					sh.ShootForwardBullet(Squares.totalSquaresInclined / 1.8f, options[i].transform.position.x, 
                        options[i].transform.position.y, options[i].transform.localScale.x, shipIndex);
					sh.ShootInclinedBullet(Squares.totalSquaresInclined / 1.8f, options[i].transform.position.x,
                        options[i].transform.position.y, options[i].transform.localScale.x, shipIndex);
				}
            }
            else
            {
                sh.ShootLaserBullet(Squares.totalSquaresInclined / 1.2f, transform.position.x, transform.position.y, transform.localScale.x, shipIndex);
				for (int i = 0; i < totalOptions; i++)
				{
                    sh.ShootLaserBullet(Squares.totalSquaresInclined / 1.2f, options[i].transform.position.x,
                        options[i].transform.position.y, options[i].transform.localScale.x, shipIndex);
				}
            }
        }
        else
        {
            sh.ShootForwardBullet(Squares.totalSquaresInclined / 1.8f, transform.position.x, transform.position.y, transform.localScale.x, shipIndex);
			for (int i = 0; i < totalOptions; i++)
			{
                sh.ShootForwardBullet(Squares.totalSquaresInclined / 1.8f, options[i].transform.position.x,
                        options[i].transform.position.y, options[i].transform.localScale.x, shipIndex);
			}
        }
    }

    void CreateOptionToShip()
    {
        options[0] = Instantiate(optionPrefab) as GameObject;
        
        Option op = options[0].GetComponent<Option>();
        op.Init();
        for (int i = 10; i >= 1; i--)
        {
            op.AddRoute(transform.position.x, transform.position.y - (speedY * Time.deltaTime * i), 10 - i);
        }
        options[0].transform.position = new Vector2(op.GetRouteX(0), op.GetRouteY(0));
        SpriteBounds.SetScaleSquare(options[0], SpriteBounds.GetSpriteWidth(gameObject) / 2f, 
            SpriteBounds.GetSpriteHeight(gameObject));
    }

    void CreateOptionToOption()
    {
        options[1] = Instantiate(optionPrefab) as GameObject;
        Option op = options[1].GetComponent<Option>();
        op.Init();
        for (int i = 10; i >= 1; i--)
        {
            op.AddRoute(options[0].transform.position.x, options[0].transform.position.y - (speedY * Time.deltaTime * i), 10 - i);
        }
        options[1].transform.position = new Vector2(op.GetRouteX(0), op.GetRouteY(0));
        SpriteBounds.SetScaleSquare(options[1], SpriteBounds.GetSpriteWidth(gameObject) / 2f,
            SpriteBounds.GetSpriteHeight(gameObject));
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
        if (shield != null)
        {
            shield.transform.position = new Vector2(
                transform.position.x + SpriteBounds.GetSpriteWidth(gameObject) / 2.0f + 
                SpriteBounds.GetSpriteWidth(shield) / 2.0f,
                transform.position.y);
        }
    }

    public void ActiveMissile(int id)
    {
        switch (id)
        {
            case 0:
                hasMissile = false;
                break;
            case 1:
                options[0].GetComponent<Option>().hasMissile = false;
                break;
            case 2:
                options[1].GetComponent<Option>().hasMissile = false;
                break;
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
        shield = Instantiate(shieldPrefab) as GameObject;
        Shield shi = shield.GetComponent<Shield>();
        shi.SetUpgradeRect(upgradeRects);
        shi.SetEnemyManager(enemyManager);
        shi.SetShipIndex(shipIndex);
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
                if (!collision.gameObject.GetComponent<Upgrade>().dead)
                {
                    collision.gameObject.GetComponent<Upgrade>().dead = true;
                    upgradeRects.AddUpgrade();
                    Destroy(collision.gameObject);
                }
                break;
            //EnemyBullets
            case 7:
                if (!invincible)
                {
                    invincible = true;
                    info.DecrementLifes(shipIndex);
                    Destroy(collision.gameObject);
                }
                break;
        }
    }

    public void RestartOptions()
    {
        for(int i = 0; i < totalOptions; i++)
        {
            Destroy(options[i]);
        }
        totalOptions = 0;
    }
    public void RestartShield()
    {
        if(shield != null)
        {
            shield.GetComponent<Shield>().SetDead(true);
            Destroy(shield);
        }
    }

    public void RestartUpgrades()
    {
        missileUpgrade = false;
        doubleUpgrade = false;
        laserUpgrade = false;
        hasMissile = false;
    }

    public void SetShipIndex(int index) { shipIndex = index; }
    public int GetShipIndex() { return shipIndex; }
    public void SetInformation(Information i) { info = i; }
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
}