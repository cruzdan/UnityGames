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

    public void Restart()
    {
        lifes--;
        if(lifes <= 0)
        {
            TotalRestart();
        }
        else
        {
            PartialRestart();
            lifesText.text = lifes.ToString();
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
        DeleteObjectsWithTag("Bullet");
        DeleteObjectsWithTag("Asteroid");
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
        PartialRestart();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
