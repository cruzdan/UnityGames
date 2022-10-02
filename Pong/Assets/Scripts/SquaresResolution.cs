using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*This class calculates the total squares X, Y and hypotenuse of the scene with respect to 
the camera height and the game window size.

Just add it to a GameObject and call SquaresResolution.<TotalSquares...> to use the variables.*/
public class SquaresResolution : MonoBehaviour
{
    public static float TotalSquaresX { get; private set; }
    public static float TotalSquaresY { get; private set; }
    public static float TotalSquaresInclined { get; private set; }
    public static SquaresResolution Instance;
    [SerializeField] Camera cam;
    private void Awake()
    {
        if (SquaresResolution.Instance == null)
        {
            SquaresResolution.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TotalSquaresY = cam.orthographicSize * 2;
        CalculateTotalSquaresX();
        CalculateTotalSquaresInclined();
    }

    void CalculateTotalSquaresX()
    {
        TotalSquaresX = TotalSquaresY * (float)Screen.width / (float)Screen.height;
    }

    void CalculateTotalSquaresInclined()
    {
        float a, b;
        a = TotalSquaresX * TotalSquaresX;
        b = TotalSquaresY * TotalSquaresY;
        TotalSquaresInclined = Mathf.Sqrt(a + b);
    }
}
