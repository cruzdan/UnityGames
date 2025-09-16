using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Logger : Singleton<Logger>
{
    [SerializeField]
    private Text debugAreaText = null;

    [SerializeField]
    private bool enableDebug = false;

    [SerializeField]
    private int maxLines = 15;

    void Awake()
    {
        if (debugAreaText == null)
        {
            debugAreaText = GetComponent<Text>();
        }
        debugAreaText.text = string.Empty;
    }

    void OnEnable()
    {
        debugAreaText.enabled = enableDebug;
        enabled = enableDebug;

        if (enabled)
        {
            debugAreaText.text += $"<color=\"white\">{DateTime.Now.ToString("HH:mm:ss.fff")} {this.GetType().Name} enabled</color>\n";
        }
    }

    public void LogInfo(string message)
    {
        ClearLines();

        debugAreaText.text += $"<color=\"blue\">{DateTime.Now.ToString("HH:mm:ss.fff")} {message}</color>\n";
    }

    public void LogError(string message)
    {
        ClearLines();
        debugAreaText.text += $"<color=\"red\">{DateTime.Now.ToString("HH:mm:ss.fff")} {message}</color>\n";
    }

    public void LogWarning(string message)
    {
        ClearLines();
        debugAreaText.text += $"<color=\"yellow\">{DateTime.Now.ToString("HH:mm:ss.fff")} {message}</color>\n";
    }

    private void ClearLines()
    {
        if (debugAreaText.text.Split('\n').Length >= maxLines)
        {
            debugAreaText.text = string.Empty;
        }
    }
}
