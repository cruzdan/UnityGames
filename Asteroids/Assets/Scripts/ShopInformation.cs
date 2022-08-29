using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInformation : MonoBehaviour
{
    [SerializeField] private Text infoText;
    [SerializeField] private Text priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button selectButton;

    [SerializeField] private GameObject mask;
    [SerializeField] private GameObject scrollBar;
    [SerializeField] private AsteroidGameManager manager;

    //Upgrades
    [SerializeField] private Shoot shoot;
    [SerializeField] private SpriteChanger spriteChanger;
    [SerializeField] private ShipMovement shipMovement;
    [SerializeField] private AsteroidsGenerator astGenerator;


    const int totalShips = 4;
    const int totalAsteroids = 8;
    const int totalBullets = 17;

    string[][] informationText;
    int[][] prices;
    bool[][] acquired;
    int[] selectedObject = new int[5];

    private int actualGlobalIndex = -1;
    private int actualParticularIndex = -1;

    private void Start()
    {
        informationText = new string[2][];
        informationText[0] = new string[totalShips];
        informationText[1] = new string[totalAsteroids];
        //the information of every bullet and extra life is the same

        prices = new int[5][];
        prices[0] = new int[totalShips];
        prices[1] = new int[totalAsteroids];
        prices[2] = new int[totalBullets];
        //extra life = 1
        prices[3] = new int[1];
        //extra bullet = 1
        prices[4] = new int[1];

        acquired = new bool[5][];
        acquired[0] = new bool[totalShips];
        acquired[1] = new bool[totalAsteroids];
        acquired[2] = new bool[totalBullets];
        acquired[3] = new bool[1];
        acquired[4] = new bool[1];

        //ships
        informationText[0][0] = "first ship to get money";
        informationText[0][1] = "increments 5% ship velocity (max speed is not included)";
        informationText[0][2] = "increments 10% ship velocity and 5% rotation speed (max speed is not included)";
        informationText[0][3] = "increments 15% ship velocity and 15% rotation speed (max speed is not included)";
        prices[0][0] = 0;
        prices[0][1] = 40;
        prices[0][2] = 60;
        prices[0][3] = 80;

        //asteroids
        informationText[1][0] = "the big asteroid (the one who generates 2 tiny asteroids) gives $2";
        for (int i = 1; i < 8; i++)
        {
            informationText[1][i] = "the big asteroid gives $" + (2 + i);
        }
        prices[1][0] = 0;
        for (int i = 1; i < 8; i++)
        {
            prices[1][i] = 10 + i * 20;
        }

        //bullets
        prices[2][0] = 0;
        for (int i = 1; i < 4; i++)
        {
            prices[2][i] = 10;
        }
        for (int i = 4; i < 8; i++)
        {
            prices[2][i] = 20;
        }
        for (int i = 8; i < 13; i++)
        {
            prices[2][i] = 30;
        }
        for (int i = 13; i < totalBullets; i++)
        {
            prices[2][i] = 40;
        }
        Init();
    }

    public void Init()
    {
        acquired[0][0] = true;
        for (int i = 1; i < totalShips; i++)
        {
            acquired[0][i] = false;
        }

        selectedObject[0] = 0;

        acquired[1][0] = true;
        for (int i = 1; i < totalAsteroids; i++)
        {
            acquired[1][i] = false;
        }

        selectedObject[1] = 0;

        acquired[2][0] = true;
        for (int i = 1; i < totalBullets; i++)
        {
            acquired[2][i] = false;
        }

        selectedObject[2] = 0;

        //extra life
        prices[3][0] = 40;
        acquired[3][0] = false;
        //selectedObject = 1 to show the buy button only
        selectedObject[3] = 1;

        //extra bullet
        prices[4][0] = 45;
        acquired[4][0] = false;
        selectedObject[4] = 1;

        if(actualGlobalIndex > -1 && actualParticularIndex > -1)
        {
            ShowButtons(actualGlobalIndex, actualParticularIndex);
        }
    }

    public void ShowInfo(int generalIndex, int particularIndex, string newText)
    {
        priceText.text = prices[generalIndex][particularIndex].ToString();
        infoText.text = newText;
    }

    /*general index: 0->ship, 1->asteroids, 2->bullet, 3->life, 4->more bullets*/
    public void ShowInfo(int generalIndex, int particularIndex)
    {
        infoText.text = informationText[generalIndex][particularIndex];
        priceText.text = prices[generalIndex][particularIndex].ToString();
    }

    /*general index: 0->ship, 1->asteroids, 2->bullet, 3->life, 4->more bullets*/
    public void ShowButtons(int generalIndex, int particularIndex)
    {
        if(selectedObject[generalIndex] == particularIndex)
        {
            buyButton.interactable = false;
            selectButton.interactable = false;
        }
        else
        {
            if (acquired[generalIndex][particularIndex])
            {
                buyButton.interactable = false;
                selectButton.interactable = true;
            }
            else
            {
                buyButton.interactable = true;
                selectButton.interactable = false;
            }
        }
    }

    public void SetParticularObjectsActive(bool value)
    {
        mask.SetActive(value);
        scrollBar.SetActive(value);
    }

    public void CleanInfoAndBuyObjects()
    {
        infoText.text = "";
        priceText.text = "";
        buyButton.interactable = false;
        selectButton.interactable = false;
    }

    public void SaveIndex(int globalIndex, int particularIndex)
    {
        actualGlobalIndex = globalIndex;
        actualParticularIndex = particularIndex;
    }

    public void BuyButton()
    {
        if(manager.GetMoney() >= prices[actualGlobalIndex][actualParticularIndex])
        {
            manager.SetMoney(manager.GetMoney() - prices[actualGlobalIndex][actualParticularIndex]);
            switch (actualGlobalIndex)
            {
                case 0:
                case 1:
                case 2:
                    acquired[actualGlobalIndex][actualParticularIndex] = true;
                    ShowButtons(actualGlobalIndex, actualParticularIndex);
                    break;
                case 3:
                    prices[actualGlobalIndex][actualParticularIndex] += prices[actualGlobalIndex][actualParticularIndex];
                    priceText.text = prices[actualGlobalIndex][actualParticularIndex].ToString();
                    manager.SetLifes(manager.GetLifes() + 1);
                    break;
                case 4:
                    IncrementBullets();
                    break;
            }
        }
    }

    public void SelectButton()
    {
        selectedObject[actualGlobalIndex] = actualParticularIndex;
        ShowButtons(actualGlobalIndex, actualParticularIndex);
        spriteChanger.ChangeSprite(actualGlobalIndex, actualParticularIndex);
        switch (actualGlobalIndex)
        {
            case 0:
                //ship upgrade
                ShipUpgrades(actualParticularIndex);
                break;
            case 1:
                //asteroid upgrade
                astGenerator.SetBonus(actualParticularIndex);
                break;
        }
    }
    void IncrementBullets()
    {
        shoot.bulletsToShoot++;
        if (shoot.bulletsToShoot > 2)
        {
            buyButton.interactable = false;
            acquired[4][0] = true;
            selectedObject[4] = 0;
        }
        else
        {
            prices[4][0] += prices[4][0];
            priceText.text = prices[4][0].ToString();
        }
    }

    public void ShipUpgrades(int option)
    {
        switch (option)
        {
            case 0:
                shipMovement.SetSpeedByPercentage(100.0f);
                shipMovement.SetAngularSpeedByPercentage(100.0f);
                break;
            case 1:
                shipMovement.SetSpeedByPercentage(105.0f);
                shipMovement.SetAngularSpeedByPercentage(100.0f);
                break;
            case 2:
                shipMovement.SetSpeedByPercentage(110.0f);
                shipMovement.SetAngularSpeedByPercentage(105.0f);
                break;
            case 3:
                shipMovement.SetSpeedByPercentage(115.0f);
                shipMovement.SetAngularSpeedByPercentage(115.0f);
                break;
        }
    }
}
