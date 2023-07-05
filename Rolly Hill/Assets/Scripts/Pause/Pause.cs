using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static event Action OnPause;
    public static event Action OnContinue;

    public void PauseGame()
    {
        Time.timeScale = 0;
        OnPause?.Invoke();
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        OnContinue?.Invoke();
    }
}
