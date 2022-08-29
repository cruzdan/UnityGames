using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradiusManager : MonoBehaviour
{
    [SerializeField] private Information info;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private GameMenu gameMenu;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private Background background;
    [SerializeField] private Level level;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private PauseManager pauseManager;
    private int actualShip = 0;
    private int deadPlayers = 0;
    private GameObject[] ship;

    public GameObject GetActualShip() { return ship[actualShip]; }
    // Start is called before the first frame update
    void Start()
    {
        pauseManager.Init(PlayerVariables.Instance.GetPlayers());

        if (PlayerVariables.Instance.GetPlayers() < 2)
        {
            ship = new GameObject[1];
            GenerateShip1();
            gameMenu.Init1Player(ship[0].GetComponent<Ship>(), KeyCode.G);
            pauseMenu.GenerateShipInfo(new Vector2(-150f, 0f), ship[0].GetComponent<Ship>());
            pauseManager.SetShip(0, ship[0]);
        }
        else
        {
            ship = new GameObject[2];
            GenerateShip1();
            GenerateShip2();
            gameMenu.Init2Players(ship[0].GetComponent<Ship>(), ship[1].GetComponent<Ship>(), KeyCode.G, KeyCode.K);
            pauseMenu.GenerateShipInfo(new Vector2(-150f, 0f), ship[0].GetComponent<Ship>());
            pauseMenu.GenerateShipInfo(new Vector2(210f, 0f), ship[1].GetComponent<Ship>());
            info.Set2Players();
            pauseManager.SetShip(0, ship[0]);
            pauseManager.SetShip(1, ship[1]);
        }
    }

    void GenerateShip1()
    {
        ship[0] = Instantiate(shipPrefab) as GameObject;
        Ship sh = ship[0].GetComponent<Ship>();
        sh.SetSpeedX(Squares.totalSquaresX / 6.0f);
        sh.SetSpeedY(0.89f * Squares.totalSquaresY / 4.0f);
        sh.SetIncrementSpeedX(Squares.totalSquaresX / 6.0f);
        sh.SetIncrementSpeedY(0.89f * Squares.totalSquaresY / 4.0f);
        sh.SetShipIndex(0);
        sh.SetInformation(info);
        sh.SetTimeInvincible(3f);
        sh.SetRightKey(KeyCode.D);
        sh.SetLeftKey(KeyCode.A);
        sh.SetUpKey(KeyCode.W);
        sh.SetDownKey(KeyCode.S);
        sh.SetShootKey(KeyCode.F);
        sh.SetEnemyManager(enemyManager);
        ship[0].transform.position = new Vector2(-Squares.totalSquaresX / 2.8f, Squares.totalSquaresY / 5.2f);
        SpriteBounds.SetScaleSquare(ship[0], Squares.totalSquaresX / 9f, Squares.totalSquaresY * 0.89f / 12f);
    }

    void GenerateShip2()
    {
        ship[1] = Instantiate(shipPrefab) as GameObject;
        Ship sh = ship[1].GetComponent<Ship>();
        sh.SetSpeedX(Squares.totalSquaresX / 6.0f);
        sh.SetSpeedY(0.89f * Squares.totalSquaresY / 4.0f);
        sh.SetIncrementSpeedX(Squares.totalSquaresX / 6.0f);
        sh.SetIncrementSpeedY(0.89f * Squares.totalSquaresY / 4.0f);
        sh.SetShipIndex(1);
        sh.SetInformation(info);
        sh.SetTimeInvincible(3f);
        sh.SetRightKey(KeyCode.RightArrow);
        sh.SetLeftKey(KeyCode.LeftArrow);
        sh.SetUpKey(KeyCode.UpArrow);
        sh.SetDownKey(KeyCode.DownArrow);
        sh.SetShootKey(KeyCode.J);
        sh.SetEnemyManager(enemyManager);
        ship[1].transform.position = new Vector2(-Squares.totalSquaresX / 2.8f, -Squares.totalSquaresY / 5.2f);
        SpriteBounds.SetScaleSquare(ship[1], Squares.totalSquaresX / 9f, Squares.totalSquaresY * 0.89f / 12f);
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
    }

    void DeleteObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }

    void Restart()
    {
        deadPlayers = 0;
        enemyManager.DeleteAllEnemies();
        DeleteObjectsWithTag("EnemyBullet");
        DeleteObjectsWithTag("Upgrade");
        DeleteObjectsWithTag("PlayerBullet");
        enemyManager.SetActualLine(0);
        actualShip = 0;
        background.ResetBackground();
        level.SetPhase(0);
        level.SetActualEnemies(0);
        level.SetTimer(0);
        background.SetPause(false);
        ship[0].transform.position = new Vector2(-Squares.totalSquaresX / 2.8f, Squares.totalSquaresY / 5.2f);

        if (PlayerVariables.Instance.GetPlayers() > 1)
        {
            ship[1].transform.position = new Vector2(-Squares.totalSquaresX / 2.8f, -Squares.totalSquaresY / 5.2f);
        }
        for (int i = 0; i < PlayerVariables.Instance.GetPlayers(); i++)
        {
            ship[i].SetActive(true);
            Ship sh = ship[i].GetComponent<Ship>();
            sh.SetSpeedX(sh.GetIncrementSpeedX());
            sh.SetSpeedY(sh.GetIncrementSpeedY());
            sh.SetBulletTimer(0f);
            sh.RestartUpgrades();
            info.SetLifes(i, 4);
            info.DecrementLifes(i);
            info.SetScore(i, 0);
            info.AddScore(i, 0);
            UpgradeRects shUpgrades = sh.GetUpgradeRects();
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
}
