using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public bool hasMissile;
    float[] routeX;
    float[] routeY;
    int index;

    public float GetRouteX(int i) { return routeX[i]; }
    public float GetRouteY(int i) { return routeY[i]; }
    public void Init()
    {
        hasMissile = false;
        routeX = new float[10];
        routeY = new float[10];
        index = 0;
    }

    public void AddRoute(float mx, float my, int i)
    {
        routeX[i] = mx;
        routeY[i] = my;
    }

    public void Move(float newX, float newY)
    {
        UpdateRoute(newX, newY);
        UpdateIndex();
        transform.position = new Vector2(routeX[index], routeY[index]);
    }

    public void UpdateRoute(float newX, float newY)
    {
        routeX[index] = newX;
        routeY[index] = newY;
    }

    public void UpdateIndex()
    {
        int a = routeX.Length;
        if (index < a - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
    }
}
