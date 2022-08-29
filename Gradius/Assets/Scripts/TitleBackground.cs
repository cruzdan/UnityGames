using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBackground : MonoBehaviour
{
    [SerializeField] private float speedX;
    private bool pause = false;
    public void SetPause(bool value) { pause = value; }
    private void Start()
    {
        speedX = Squares.totalSquaresX / 5f;
        transform.position = new Vector2(-Squares.totalSquaresX, 0f);
    }
    // Update is called once per frame
    void Update()
    {
        if(!pause)
        {
            Move();
        }
    }

    public void Move()
    {
        transform.Translate(new Vector2(speedX * Time.deltaTime, 0f));
    }
}
