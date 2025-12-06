using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Private Variables
    private float totalTime;
    private float timer;
    private int currentIntNumber;
    #endregion
    #region Actions
    public Action OnTimerCompleted;
    public Action<int> OnIntNumberChanged;
    #endregion
    #region Functions
    public void StartTimer(float time)
    {
        totalTime = time;
        currentIntNumber = Mathf.CeilToInt(totalTime);
        OnIntNumberChanged?.Invoke(currentIntNumber + 1);
        timer = time;
        enabled = true;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (currentIntNumber != (int) timer)
        {
            currentIntNumber = (int)timer;
            OnIntNumberChanged?.Invoke(currentIntNumber + 1);
        }
        if (timer <= 0)
        {
            TimerCompleted();
        }
    }

    private void TimerCompleted()
    {
        enabled = false;
        OnTimerCompleted?.Invoke();
    }
    #endregion
}
