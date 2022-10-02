using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        counter.gameObject.SetActive(false);
        counter.SetTime(-1f);
    }
    public void Restart()
    {
        lifes--;
        if (lifes <= 0)
        {
            TotalRestart();
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
    public void TotalRestart()
    {
        lifes = 3;
        lifesText.text = lifes.ToString();
        money = 0;
        moneyText.text = money.ToString();
        shopMoneyText.text = money.ToString();
        ship.GetComponent<ShipMovement>().InitSpeed();
        ship.GetComponent<ShipMovement>().InitRotation();
        ship.GetComponent<Shoot>().bulletsToShoot = 1;
        astGen.SetBonus(0);
        spriteChanger.Init();
        shopInformation.Init();
        pauseManager.GameOverChange();
        PartialRestart();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
