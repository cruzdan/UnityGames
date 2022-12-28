using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    [SerializeField] private float timeToSetVisible;
    [SerializeField] private SpriteRenderer spriteRenderer;
    bool visible = true;
    Color currentColor;
    private float timer;
    bool pause = false;
    private void Start()
    {
        currentColor = spriteRenderer.color;
        timer = timeToSetVisible;
    }
    private void Update()
    {
        if (!pause)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (visible)
                {
                    currentColor.a = 0;
                }
                else
                {
                    currentColor.a = 255;
                }
                spriteRenderer.color = currentColor;
                timer = timeToSetVisible;
                visible = !visible;
            }
        }
    }
    public void SetPause(bool value) { pause = value; }
    public void Restart()
    {
        currentColor = spriteRenderer.color;
        currentColor.a = 255;
        timer = timeToSetVisible;
        visible = true;
        spriteRenderer.color = currentColor;
    }
    public void SetVisible(bool value)
    {
        visible = value;
        if (visible)
        {
            currentColor.a = 255;
        }
        else
        {
            currentColor.a = 0;
        }
        spriteRenderer.color = currentColor;
    }
    public void SetCurrentColor(Color newColor)
    {
        currentColor = newColor;
    }
}
