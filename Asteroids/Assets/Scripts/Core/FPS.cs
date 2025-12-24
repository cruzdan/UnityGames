using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    private Text fpsText;

    private void Start()
    {
        fpsText = GetComponent<Text>();
        GetFPS();
        InvokeRepeating("GetFPS", 1, 1);
    }

    public void GetFPS()
    {
        fpsText.text = (int)(1.0f / Time.unscaledDeltaTime) + " FPS";
    }
}
