using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeKeyManager : MonoBehaviour
{
    [SerializeField] private Text text;
    private PlayerMovement playerMovement;
    [SerializeField] private ControlMenuButtons controlMenuButtons;
    bool up = true;

    public void SetPlayerMovement(int player)
    {
        if(player == 1)
        {
            playerMovement = GameObject.Find("LeftPaddle").GetComponent<PlayerMovement>();
        }else
            playerMovement = GameObject.Find("RightPaddle").GetComponent<PlayerMovement>();
    }

    public void OnGUI()
    {
        if (Input.anyKeyDown)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                if (e.keyCode != KeyCode.Escape && e.keyCode != KeyCode.None) 
                {
                    text.text = e.keyCode.ToString();
                    if (up)
                    {
                        playerMovement.up = e.keyCode;
                    }
                    else
                    {
                        playerMovement.down = e.keyCode;
                    }
                    controlMenuButtons.ChangeActiveButtons();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetActiveChangeKeyPanel()
    {
        gameObject.SetActive(true);
    }
    public void UpKeyControl(bool isUp)
    {
        up = isUp;
    }

    public void SetTextField(Text textField)
    {
        text = textField;
    }
}
