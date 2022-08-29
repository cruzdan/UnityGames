using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private KeyCode pauseKey;
    [SerializeField] private GameObject pauseMenu;
    private bool pause = false;
    private int total;
    private GameObject[] ships;

    public void Init(int totalShips)
    {
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
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (pause)
        {
            pause = false;
            Time.timeScale = 1f;
        }
        else
        {
            pause = true;
            Time.timeScale = 0f;
        }
        ActiveShips(!pause);
        pauseMenu.SetActive(pause);
    }

    public void ActiveShips(bool active)
    {
        for (int i = 0; i < total; i++)
        {
            ships[i].SetActive(active);
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
