using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GradiusManager : MonoBehaviour
{
    [SerializeField] private GameMenuInformation info;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private UpgradeGameMenuCreator gameMenu;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private BackgroundMovement background;
    [SerializeField] private LevelInfo level;
    [SerializeField] private ControlsMenuCreator controlsMenu;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private ChangeControlsManager selectKey;
    /*pools to: 0 -> forward bullets, 1 -> inclined bullets, 2 -> laser bullets, 3 -> enemy bullets, 4 -> upgrades*/ 
    [SerializeField] private ObjectPool[] pools;
    [SerializeField] private CounterBack counter;
    private int actualShip = 0;
    private int deadPlayers = 0;
    private GameObject[] ship;

    public GameObject GetActualShip() { return ship[actualShip]; }
    void Start()
    {
        Application.targetFrameRate = 60;
        pauseManager.Init(PlayerVariables.Instance.GetPlayers());

        if (PlayerVariables.Instance.GetPlayers() < 2)
        {
            ship = new GameObject[1];
            CreateInitialShip(0, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.Z, KeyCode.X,
                -SquaresResolution.TotalSquaresX / 2.8f, SquaresResolution.TotalSquaresY / 5.2f, 0f, -185f, 1.3f, 0f, -15f);
            controlsMenu.SetActiveShip2Title(false);
            if (Joystick.all.Count > 0)
            {
                SetShipInputDevice(0, Joystick.all[0]);
            }
            else if (Gamepad.all.Count > 0)
            {
                SetShipInputDevice(0, Gamepad.all[0]);
            }
        }
        else
        {
            ship = new GameObject[2];

            CreateInitialShip(0, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.Z, KeyCode.X,
                -SquaresResolution.TotalSquaresX / 2.8f, SquaresResolution.TotalSquaresY / 5.2f, 0f, -175f, 1f, -199f, -15f);

            CreateInitialShip(1, KeyCode.L, KeyCode.J, KeyCode.I, KeyCode.K, KeyCode.T,
                KeyCode.Y,
                -SquaresResolution.TotalSquaresX / 2.8f, -SquaresResolution.TotalSquaresY / 5.2f, 0f, -195f, 1f, 199f, -15f);

            info.Set2Players();
            int index = 0;
            int totalJoysticks = Joystick.all.Count;
            int totalGamepads = Gamepad.all.Count;
            for (int i = 0; i < totalJoysticks; i++)
            {
                SetShipInputDevice(index, Joystick.all[i]);
                index++;
                if(index > 1)
                {
                    return;
                }
            }
            for(int i = 0; i < totalGamepads; i++)
            {
                SetShipInputDevice(index, Gamepad.all[i]);
                index++;
                if (index > 1)
                {
                    return;
                }
            }
        }
    }
    void SetShipInputDevice(int shipIndex, InputDevice inputDevice)
    {
        ship[shipIndex].GetComponent<Ship>().SetInputDevice(inputDevice);
        ship[shipIndex].GetComponent<Ship>().GetUpgradeRectsManager().SetInputDevice(inputDevice);
    }
    void CreateInitialShip(int shipIndex, KeyCode right, KeyCode left, KeyCode up, KeyCode down, KeyCode shoot,
        KeyCode select,
        float shipPositionX, float shipPositionY, float upgradePositionX, float upgradePositionY, float  upgradeScaleY,
        float infoPositionX, float infoPositionY)
    {
        GenerateShip(shipIndex, right, left, up, down, shoot, shipPositionX, shipPositionY);
        gameMenu.CreateUpgrade(ship[shipIndex].GetComponent<Ship>(), select, upgradePositionX, upgradePositionY, upgradeScaleY);
        selectKey.AddShip(ship[shipIndex].GetComponent<Ship>());
        controlsMenu.GenerateShipInfo(new Vector2(infoPositionX, infoPositionY), ship[shipIndex].GetComponent<Ship>());
        pauseManager.SetShip(shipIndex, ship[shipIndex]);
    }
    void GenerateShip(int shipIndex, KeyCode right, KeyCode left, KeyCode up, KeyCode down, KeyCode shoot,
        float positionX, float positionY)
    {
        ship[shipIndex] = Instantiate(shipPrefab) as GameObject;
        Ship sh = ship[shipIndex].GetComponent<Ship>();
        sh.SetSpeedX(SquaresResolution.TotalSquaresX / 6.0f);
        sh.SetSpeedY(0.89f * SquaresResolution.TotalSquaresY / 4.0f);
        sh.SetIncrementSpeedX(SquaresResolution.TotalSquaresX / 6.0f);
        sh.SetIncrementSpeedY(0.89f * SquaresResolution.TotalSquaresY / 4.0f);
        sh.SetShipIndex(shipIndex);
        sh.SetInformation(info);
        sh.SetTimeInvincible(3f);
        sh.SetRightKey(right);
        sh.SetLeftKey(left);
        sh.SetUpKey(up);
        sh.SetDownKey(down);
        sh.SetShootKey(shoot);
        sh.SetEnemyManager(enemyManager);
        ship[shipIndex].GetComponent<Shoot>().SetPools(pools);
        ship[shipIndex].transform.position = new Vector2(positionX, positionY);
        SpriteBounds.SetScaleSquare(ship[shipIndex], SquaresResolution.TotalSquaresX / 9f, 
            SquaresResolution.TotalSquaresY * 0.89f / 12f);
    }

    public void UpdateActualShipIndex()
    {
        if (deadPlayers > 0)
        {
            int i;
            for (i = 0; i < PlayerVariables.Instance.GetPlayers(); i++)
            {
                if (!ship[i].GetComponent<Ship>().GetDead())
                {
                    break;
                }
            }
            actualShip = i;
        }
        else
        {
            if (actualShip == 0)
            {
                actualShip = 1;
            }
            else
            {
                actualShip = 0;
            }
        }
    }
    public void ShipDead(int shipIndex)
    {
        deadPlayers++;
        Vector2 pos = ship[shipIndex].transform.position;
        if(deadPlayers >= PlayerVariables.Instance.GetPlayers())
        {
            Restart();
        }
        else
        {
            Ship sh = ship[shipIndex].GetComponent<Ship>();
            sh.SetDead(true);
            ship[shipIndex].SetActive(false);
            UpdateActualShipIndex();
            enemyManager.ChangeShipTargetToEnemies(GetActualShip());
            sh.RestartOptions();
            sh.RestartShield();
        }
        ParticleManager.Instance.PlayParticleSystem(pos);
    }
    void ReturnPlayerBulletsToPool()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PlayerBullet");
        for (int i = 0; i < objects.Length; i++)
        {
            
            if (objects[i].layer != 15)
            {
                //forward, inclined and laser bullets
                objects[i].GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(objects[i]);
            }
            else
            {
                //missile bullets
                objects[i].SetActive(false);
            }
        }
    }

    void ReturnPoolObjectsWithTag(string tag, ObjectPool pool)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < objects.Length; i++)
        {
            pool.ReturnObjectToPool(objects[i]);
        }
    }
    void ReturnEnemyToPool(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(objects[i]);
        }
    }

    public void ReturnToPollAllEnemies()
    {
        ReturnEnemyToPool("Enemy0");
        ReturnEnemyToPool("Enemy1");
        ReturnEnemyToPool("Enemy2");
        ReturnEnemyToPool("Enemy3");
        ReturnEnemyToPool("Enemy4");
        ReturnEnemyToPool("Enemy5");
        ReturnEnemyToPool("Enemy5up");
        ReturnEnemyToPool("Enemy6");
        ReturnEnemyToPool("Enemy7");
        ReturnEnemyToPool("Enemy8");
        level.GetComponent<EnemyGenerator>().SetActiveEnemy9(false);
    }

    void Restart()
    {
        deadPlayers = 0;
        ReturnToPollAllEnemies();
        ReturnPoolObjectsWithTag("EnemyBullet", pools[3]);
        ReturnPoolObjectsWithTag("Upgrade", pools[4]);
        ParticleManager.Instance.ReturnAllParticles();
        ReturnPlayerBulletsToPool();
        enemyManager.SetActualLine(0);
        actualShip = 0;
        background.ResetBackground();
        level.SetPhase(0);
        level.SetActualEnemies(0);
        level.SetTimer(0);
        counter.Reiniciate();
        background.SetPause(true);
        ship[0].transform.position = new Vector2(-SquaresResolution.TotalSquaresX / 2.8f, 
            SquaresResolution.TotalSquaresY / 5.2f);

        if (PlayerVariables.Instance.GetPlayers() > 1)
        {
            ship[1].transform.position = new Vector2(-SquaresResolution.TotalSquaresX / 2.8f, 
                -SquaresResolution.TotalSquaresY / 5.2f);
        }
        for (int i = 0; i < PlayerVariables.Instance.GetPlayers(); i++)
        {
            ship[i].SetActive(false);
            Ship sh = ship[i].GetComponent<Ship>();
            sh.SetSpeedX(sh.GetIncrementSpeedX());
            sh.SetSpeedY(sh.GetIncrementSpeedY());
            sh.SetBulletTimer(0f);
            sh.RestartUpgrades();
            info.SetLifes(i, 4);
            info.DecrementLifes(i);
            info.SetScore(i, 0);
            info.AddScore(i, 0);
            UpgradeRectsManager shUpgrades = sh.GetUpgradeRects();
            shUpgrades.RestartGettedUpgrades();
            shUpgrades.InitRects();
            shUpgrades.SetActualUpgrade(-1);
            sh.RestartOptions();
            sh.RestartShield();
            sh.SetInvincible(false);
            sh.SetTimerInvincible(sh.GetTimeInvincible());
            sh.SetVisible(true);
            sh.SetDead(false);
        }
    }
    public void ChangeRestartingToPlayGame()
    {
        background.SetPause(false);
        for (int i = 0; i < PlayerVariables.Instance.GetPlayers(); i++)
        {
            ship[i].SetActive(true);
        }
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}