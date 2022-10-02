using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonicWaveMovement : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private float longitude;
    [SerializeField] private float center;
    float endX;
    public void SetSpeedX(float newSpeedX) { speedX = newSpeedX; }
    public void SetLongitude(float newLongitude) { longitude = newLongitude; }
    public void SetCenter(float newCenter) { center = newCenter; }
    public float GetSpeedX() { return speedX; }
    public float GetLongitude() { return longitude; }
    public float GetCenter() { return center; }

    // Start is called before the first frame update
    void Start()
    {
        endX = SquaresResolution.TotalSquaresX;
    }

    public void Init(float newSpeedX, float newLongitude, float newCenter)
    {
        speedX = newSpeedX;
        longitude = newLongitude;
        center = newCenter;
    } 

    private void FixedUpdate()
    {
        float posX = transform.position.x + speedX * Time.fixedDeltaTime;
        float distance = endX / 2.0f - posX;
        float grades = (endX - distance) * 720f / endX;
        float posY = longitude * Mathf.Sin( grades * Mathf.PI / 180.0f)  + center;
        transform.position = new Vector2(posX, posY);
    }
}
