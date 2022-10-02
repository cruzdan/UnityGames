using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private KeyCode pauseKey;
    //0 -> Game, 1 -> Pause, 2 -> Controls, 3 -> SelectControlKey
    [SerializeField] private GameObject[] menus;
    [SerializeField] private GameMenuInformation info;
    //0 -> Game, 1 -> Pause, 2 -> Controls, 3 -> SelectControlKey
    private int menuIndex = 0;
    private int total;
    private GameObject[] ships;

    public void Init(int totalShips)
    {
        total = totalShips;
        if(totalShips < 2)
        {
            ships = new GameObject[1];
        }
        else
        {
            ships = new GameObject[2];
        }
    }

    public void SetShip(int shipIndex, GameObject ship)
    {
        ships[shipIndex] = ship;
    }
    public void SetMenuIndex(int index)
    {
        menuIndex = index;
    }
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            Pause();
        }
    }

    public void Pause()
    {
        switch (menuIndex)
        {
            case 0:
                //from Game menu pass to Pause menu
                menuIndex = 1;
                Time.timeScale = 0f;
                ActivateShips(false);
                menus[0].SetActive(false);
                menus[1].SetActive(true);
                
                break;
            case 1:
                //from Pause menu pass to Game menu
                menuIndex = 0;
                Time.timeScale = 1f;
                ActivateShipFromPause();
                menus[0].SetActive(true);
                menus[1].SetActive(false);
                break;
            case 2:
                //from Controls menu to Pause menu
                menus[2].SetActive(false);
                menus[1].SetActive(true);
                menuIndex = 1;
                break;
        }
    }

    public void ChangeFromControlsToSelectMenu()
    {
        menus[3].SetActive(true);
        menus[2].SetActive(false);
        menuIndex = 3;
    }
    public void ChangeFromSelectToControlsMenu()
    {
        menus[2].SetActive(true);
        menus[3].SetActive(false);
        menuIndex = 2;
    }
    public void ActivateShips(bool active)
    {
        for (int i = 0; i < total; i++)
        {
            ships[i].SetActive(active);
        }
    }
    public void ActivateShipFromPause()
    {
        for (int i = 0; i < total; i++)
        {
            if (info.GetLifes(i) > 0)
            {
                ships[i].SetActive(true);
            }
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
