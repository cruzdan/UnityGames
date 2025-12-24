using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteCommands : MonoBehaviour
{
    private Text commandText;
    [SerializeField] GameObject allCommandsShield;
    private void Start()
    {
        commandText = GetComponent<Text>();
    }
    public void OnGUI()
    {
        if (Input.anyKeyDown)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                if (e.keyCode > KeyCode.BackQuote && e.keyCode < KeyCode.LeftCurlyBracket)
                {
                    if (e.keyCode != KeyCode.H)
                    {
                        commandText.text = commandText.text + e.keyCode.ToString().ToLower();
                    }
                    else
                    {
                        if (allCommandsShield.activeSelf)
                        {
                            allCommandsShield.SetActive(false);
                        }
                        else
                        {
                            allCommandsShield.SetActive(true);
                        }
                    }
                }
                else if (e.keyCode == KeyCode.Space)
                {
                    commandText.text = commandText.text + " ";
                }
                else if (e.keyCode == KeyCode.Backspace)
                {
                    if (commandText.text.Length > 0)
                    {
                        commandText.text = commandText.text.Remove(commandText.text.Length - 1);
                    }
                }
            }
        }
    }
}
