using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    
    [SerializeField] private GameObject[] menus;
    [SerializeField] private CounterBack counter;
    //0 -> game, 1 -> pause, 2 -> shop, 3 -> game over
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

    public void PauseGame()
    {
        pause = true;
        Time.timeScale = 0.0f;
    }
    public void ContinueGame()
    {
        pause = false;
        Time.timeScale = 1.0f;
    }

    public void ChangePauseMenu()
    {
        switch (menuIndex)
        {
            case 0:
                counter.gameObject.SetActive(false);
                menuIndex = 1;
                menus[0].SetActive(false);
                menus[1].SetActive(true);
                PauseGame();
                break;
            case 1:
                menuIndex = 0;
                menus[1].SetActive(false);
                menus[0].SetActive(true);
                Time.timeScale = 1.0f;
                if (counter.GetTime() > 0f)
                {
                    pause = true;
                    counter.gameObject.SetActive(true);
                }
                else
                {
                    pause = false;
                }
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

    public void GameOverChange()
    {
        if (pause)
        {
            //change from game over to playing
            menuIndex = 0;
            menus[3].SetActive(false);
            menus[0].SetActive(true);
            Time.timeScale = 1.0f;
            counter.Reiniciate();
            counter.gameObject.SetActive(true);
        }
        else
        {
            //change from playing to game over
            menuIndex = 3;
            menus[3].SetActive(true);
            menus[0].SetActive(false);
            PauseGame();
        }
    }
}
