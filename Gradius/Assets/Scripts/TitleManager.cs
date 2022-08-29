using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private TitleBackground titleBackground;
    [SerializeField] private GameObject ship;
    private int phase = 0;
    int totalPlayers = 1;
    float posY1;
    float posY2;

    private void Start()
    {
        SpriteBounds.SetScaleSquare(ship, Squares.totalSquaresX / 9f, Squares.totalSquaresY * 0.89f / 12f);
        float x = Squares.totalSquaresX * 4.82f / 15f;
        float y = Squares.totalSquaresY * 5.73f / 10f;
        posY1 = Squares.totalSquaresY / 2f - y;
        y = Squares.totalSquaresY * 6.45f / 10f;
        posY2 = Squares.totalSquaresY / 2f - y;
        ship.transform.position = new Vector2(-Squares.totalSquaresX / 2f + x, posY1);
        ship.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(phase == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                titleBackground.transform.position = Vector2.zero;
                phase = 1;
                titleBackground.SetPause(true);
                ship.SetActive(true);
            }
            if (titleBackground.transform.position.x > 0f)
            {
                phase = 1;
                titleBackground.SetPause(true);
                ship.SetActive(true);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                totalPlayers = 1;
                ship.transform.position = new Vector2(ship.transform.position.x, posY1);

            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                totalPlayers = 2;
                ship.transform.position = new Vector2(ship.transform.position.x, posY2);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerVariables.Instance.SetPlayers(totalPlayers);
                SceneManager.LoadScene("GradiusScene");
            }
        }
    }
}
