using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    
    [SerializeField] private GameObject[] menus;
    //0 -> game, 1 -> pause, 2 -> shop
    int menuIndex = 0;
    public bool pause = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePauseMenu();
        }
    }

    public void ChangePauseMenu()
    {
        switch (menuIndex)
        {
            case 0:
                menuIndex = 1;
                menus[0].SetActive(false);
                menus[1].SetActive(true);
                Time.timeScale = 0.0f;
                pause = true;
                break;
            case 1:
                menuIndex = 0;
                menus[1].SetActive(false);
                menus[0].SetActive(true);
                Time.timeScale = 1.0f;
                pause = false;
                break;
            case 2:
                menuIndex = 1;
                menus[2].SetActive(false);
                menus[1].SetActive(true);
                break;
        }
    }

    public void ShopMenuButton()
    {
        menuIndex = 2;
        menus[1].SetActive(false);
        menus[2].SetActive(true);
    }
}
