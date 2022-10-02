using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonFunctions : MonoBehaviour
{
    [SerializeField] private ShopInformation shopInf;
    [SerializeField] Button[] shipButtons;
    [SerializeField] Button[] asteroidButtons;
    [SerializeField] Button[] bulletButtons;
    // Start is called before the first frame update
    void Start()
    {
        int total = shipButtons.Length;
        for (int i = 0; i < total; i++)
        {
            //this variable is created to not change the value in the next functions
            int m = i;
            shipButtons[i].onClick.AddListener(() => { shopInf.ShowInfo(0, m); });
            shipButtons[i].onClick.AddListener(() => { shopInf.ShowButtons(0, m); });
            shipButtons[i].onClick.AddListener(() => { shopInf.SaveIndex(0, m); });
        }

        total = asteroidButtons.Length;
        for (int i = 0; i < total; i++)
        {
            int m = i;
            asteroidButtons[i].onClick.AddListener(() => { shopInf.ShowInfo(1, m); });
            asteroidButtons[i].onClick.AddListener(() => { shopInf.ShowButtons(1, m); });
            asteroidButtons[i].onClick.AddListener(() => { shopInf.SaveIndex(1, m); });
        }

        total = bulletButtons.Length;
        
        for (int i = 0; i < total; i++)
        {
            int m = i;
            //the info is the same to every bullet
            bulletButtons[m].onClick.AddListener(() => { shopInf.ShowInfo(2, m, shopInf.GetShopInformation(12)); });
            bulletButtons[m].onClick.AddListener(() => { shopInf.ShowButtons(2, m); });
            bulletButtons[i].onClick.AddListener(() => { shopInf.SaveIndex(2, m); });
        }

        Destroy(this);
    }
}
