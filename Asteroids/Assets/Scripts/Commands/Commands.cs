using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commands : MonoBehaviour
{
    [SerializeField] private GameObject textField;
    [SerializeField] private Text commandText;
    [SerializeField] private Text fpsText;
    [SerializeField] private AsteroidGameManager manager;
    string[] commands;

    private void Start()
    {
        commands = new string[4];
        commands[0] = "add money";
        commands[1] = "add life";
        commands[2] = "fps count on";
        commands[3] = "fps count off";
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            if (!textField.activeSelf)
            {
                textField.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {

            if (textField.activeSelf)
            {
                int index = CheckSameCommands(commandText.text);
                if (index >= 0)
                {
                    GenerateCommand(index);
                }
                commandText.text = "";
                textField.SetActive(false);
            }
        }
    }

    //check if the text is a command, return the index of the command if it exists, else it returns -1
    int CheckSameCommands(string text)
    {
        int index = -1;
        int size = commands.Length;
        for (int i = 0; i < size; i++)
        {
            if (text.Equals(commands[i]))
            {
                index = i;
                break;
            }
        }
        return index;
    }

    void GenerateCommand(int index)
    {
        switch (index)
        {
            case 0:
                manager.SetMoney(manager.GetMoney() + 100);
                break;
            case 1:
                manager.SetLifes(manager.GetLifes() + 1);
                break;
            case 2:
                fpsText.gameObject.SetActive(true);
                break;
            case 3:
                fpsText.gameObject.SetActive(false);
                break;
        }
    }
}
