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
    string[] fileInformation;

    private int actualGlobalIndex = -1;
    private int actualParticularIndex = -1;

    private void Start()
    {
        fileInformation = FileReader.GetContentFromFileBuild("ShopInformation.txt");

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
        int fileIndex = 0;
        for(int i = 0; i < totalShips; i++)
        {
            informationText[0][i] = fileInformation[fileIndex];
            fileIndex++;
        }
        //asteroids
        for (int i = 0; i < totalAsteroids; i++)
        {
            informationText[1][i] = fileInformation[fileIndex];
            fileIndex++;
        }

        //ships
        fileIndex = 15;
        for(int i = 0; i < totalShips; i++)
        {
            int.TryParse(fileInformation[fileIndex], out prices[0][i]);
            fileIndex++;
        }
        //asteroids
        for(int i = 0; i < totalAsteroids; i++)
        {
            int.TryParse(fileInformation[fileIndex], out prices[1][i]);
            fileIndex++;
        }
        //bullets
        for (int i = 0; i < totalBullets; i++)
        {
            int.TryParse(fileInformation[fileIndex], out prices[2][i]);
            fileIndex++;
        }
        Init();
    }

    public void Init()
    {
        priceText.text = "";
        infoText.text = "";
        
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
        int.TryParse(fileInformation[44], out prices[3][0]);
        acquired[3][0] = false;
        //selectedObject = 1 to show the buy button only
        selectedObject[3] = 1;

        //extra bullet
        int.TryParse(fileInformation[45], out prices[4][0]);
        acquired[4][0] = false;
        selectedObject[4] = 1;

        if(actualGlobalIndex > -1 && actualParticularIndex > -1)
        {
            ShowButtons(actualGlobalIndex, actualParticularIndex);
        }

        buyButton.interactable = false;
    }
    public string GetShopInformation(int index)
    {
        return fileInformation[index];
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
