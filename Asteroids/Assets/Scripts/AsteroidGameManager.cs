using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsteroidGameManager : MonoBehaviour
{
    [SerializeField] private GameObject ship;
    [SerializeField] private Text lifesText;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text shopMoneyText;
    [SerializeField] private AsteroidsGenerator astGen;
    [SerializeField] private SpriteChanger spriteChanger;
    [SerializeField] private ShopInformation shopInformation;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private CounterBack counter;
    //0 -> asteroidPool, 1 -> bulletPool
    [SerializeField] private ObjectPool[] objectPools;
    //inital bullets when start game and restart game
    [SerializeField] private int initalBullets = 1;
    [SerializeField] private bool inGame;
    private int lifes = 3;
    private int money = 0;
    public void SetLifes(int newLifes)
    {
        lifes = newLifes;
        lifesText.text = lifes.ToString();
    }
    public int GetLifes()
    {
        return lifes;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        if (inGame) 
        { 
            counter.SetTime(-1); 
            TotalRestart(false); 
        } 
    }
    public void Restart()
    {
        lifes--;
        if (lifes <= 0)
        {
            TotalRestart(true);
            counter.gameObject.SetActive(false);
        }
        else
        {
            PartialRestart();
            pauseManager.pause = true;
            counter.Reiniciate();
            counter.gameObject.SetActive(true);
            lifesText.text = lifes.ToString();
        }

    }
    void ReturnObjectsToPool(string tag, ObjectPool obPool)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        int total = objects.Length;
        for(int i = 0; i < total; i++)
        {
            obPool.ReturnObjectToPool(objects[i]);
        }
    }

    public void SetMoney(int newMoney)
    {
        money = newMoney;
        moneyText.text = money.ToString();
        shopMoneyText.text = money.ToString();
    }

    public int GetMoney()
    {
        return money;
    }

    void PartialRestart()
    {
        ReturnObjectsToPool("Bullet", objectPools[1]);
        ReturnObjectsToPool("Asteroid", objectPools[0]);
        ship.GetComponent<ShipMovement>().Restart();
        ship.GetComponent<Shoot>().Restart();
    }
    public void TotalRestart(bool toPause)
    {
        lifes = 3;
        lifesText.text = lifes.ToString();
        money = 0;
        moneyText.text = money.ToString();
        shopMoneyText.text = money.ToString();
        ship.GetComponent<ShipMovement>().InitSpeed();
        ship.GetComponent<ShipMovement>().InitRotation();
        ship.GetComponent<Shoot>().bulletsToShoot = initalBullets;
        astGen.SetBonus(0);
        spriteChanger.Init();
        shopInformation.Init();
        if(toPause)
            pauseManager.GameOverChange();
        PartialRestart();
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void ChangeGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
