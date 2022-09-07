using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE(isaveg): Explain briefly what is this class for and how it is used
public class Squares : MonoBehaviour
{
    public static float totalSquaresX;
    public static float totalSquaresY;
    public static float totalSquaresInclined;
    // Start is called before the first frame update
    void Start()
    {
        CalculateTotalSquaresX();
        totalSquaresY = 10.0f;
        CalculateTotalSquaresInclined();
    }

    void CalculateTotalSquaresX()
    {
        totalSquaresX = 10f * (float)Screen.width / (float)Screen.height;
    }

    void CalculateTotalSquaresInclined()
    {
        float a, b;
        a = totalSquaresX * totalSquaresX;
        b = totalSquaresY * totalSquaresY;
        totalSquaresInclined = Mathf.Sqrt(a + b);
    }
}
