using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    //last position in x to return the background to the begining
    private float lastPositionX;
    private bool pause = false;

    public void SetPause(bool newPause) { pause = newPause; }
    
    // Start is called before the first frame update
    void Start()
    {
        //these values are obtained by the pixels of the background image, width and height 

        transform.localScale = new Vector2(SquaresResolution.TotalSquaresX * 3624.0f / 224.0f, 
            SquaresResolution.TotalSquaresY * 0.89f);
        transform.position = new Vector2(transform.localScale.x / 2.0f - SquaresResolution.TotalSquaresX / 2.0f, 0.55f);
        speed = SquaresResolution.TotalSquaresX / 7.0f;
        lastPositionX = -(3326f * transform.localScale.x / 3624f - transform.localScale.x / 2f + 
            SquaresResolution.TotalSquaresX / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
    }
    public bool IsOnEnd()
    {
        return transform.position.x <= lastPositionX;
    }

    public void ResetBackground()
    {
        transform.position = new Vector2(transform.localScale.x / 2.0f - SquaresResolution.TotalSquaresX / 2.0f, 
            transform.position.y);
    }
}
