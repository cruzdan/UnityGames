using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squares : MonoBehaviour
{
    public static float totalSquaresX;
    public static float totalSquaresY;
    public static float totalSquaresInclined;
    [SerializeField] Camera cam;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        totalSquaresY = cam.orthographicSize * 2;
        CalculateTotalSquaresX();
        CalculateTotalSquaresInclined();
    }

    void CalculateTotalSquaresX()
    {
        totalSquaresX = totalSquaresY * (float)Screen.width / (float)Screen.height;
    }

    void CalculateTotalSquaresInclined()
    {
        float a, b;
        a = totalSquaresX * totalSquaresX;
        b = totalSquaresY * totalSquaresY;
        totalSquaresInclined = Mathf.Sqrt(a + b);
    }
}
