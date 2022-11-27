using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private TitleBackgroundMovement titleBackground;
    [SerializeField] private GameObject ship;
    private int phase = 0;
    int totalPlayers = 1;
    float posY1;
    float posY2;
    PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        SpriteBounds.SetScaleSquare(ship, SquaresResolution.TotalSquaresX / 9f, SquaresResolution.TotalSquaresY * 0.89f / 12f);
        float x = SquaresResolution.TotalSquaresX * 4.82f / 15f;
        float y = SquaresResolution.TotalSquaresY * 5.73f / 10f;
        posY1 = SquaresResolution.TotalSquaresY / 2f - y;
        y = SquaresResolution.TotalSquaresY * 6.45f / 10f;
        posY2 = SquaresResolution.TotalSquaresY / 2f - y;
        ship.transform.position = new Vector2(-SquaresResolution.TotalSquaresX / 2f + x, posY1);
        ship.SetActive(false);
    }
    void Update()
    {
        if(phase == 0)
        {
            if (Input.GetButtonDown("Jump") || playerInput.actions["Shoot"].WasPressedThisFrame())
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
            if(Input.GetAxis("Vertical") > 0)
            {
                totalPlayers = 1;
                ship.transform.position = new Vector2(ship.transform.position.x, posY1);

            }
            if (Input.GetAxis("Vertical") < 0)
            {
                totalPlayers = 2;
                ship.transform.position = new Vector2(ship.transform.position.x, posY2);
            }
            if (Input.GetButtonDown("Jump") || playerInput.actions["Shoot"].WasPressedThisFrame())
            {
                PlayerVariables.Instance.SetPlayers(totalPlayers);
                SceneManager.LoadScene("GradiusScene");
            }
        }
    }
}
