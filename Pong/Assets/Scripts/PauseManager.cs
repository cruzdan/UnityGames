using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameMenu;
    bool pause = false;
    bool otherMenu = false;


    public void ChangeOtherMenu()
    {
        otherMenu = !otherMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !otherMenu)
        {
            ChangePauseMenu();
        }
    }

    public void ChangePauseMenu() {
        if (pause)
        {
            pauseMenu.SetActive(false);
            gameMenu.SetActive(true);
            Time.timeScale = 1;
        }
        else
        {
            pauseMenu.SetActive(true);
            gameMenu.SetActive(false);
            Time.timeScale = 0;
        }
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayButtonSound();
        pause = !pause;
    }
}
