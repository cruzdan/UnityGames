using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandsUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Button commandsButton;
    [SerializeField] private GameObject commandLineObject;
    [SerializeField] private TMP_InputField commandInputField;
    [SerializeField] private Button enterCommandsButton;
    [SerializeField] private GameObject commandCanvasObject;
    #endregion
    #region Actions
    public Action<string> OnCommandSubmit;
    #endregion
    #region Functions
    private void Start()
    {
        commandsButton.onClick.AddListener(ToggleCommandLine);
        commandLineObject.SetActive(false);
        commandInputField.onSubmit.AddListener(SubmitCommand);
        enterCommandsButton.onClick.AddListener(() => SubmitCommand(commandInputField.text));
    }

    private void ToggleCommandLine()
    {
        ActiveCommandLine(!commandLineObject.activeSelf);
    }

    public void SubmitCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command)) return;
        commandInputField.text = string.Empty;
        commandLineObject.SetActive(false);
        OnCommandSubmit?.Invoke(command);
    }

    public void ActiveCommandCanvas(bool value)
    {
        commandCanvasObject.SetActive(value);
    }

    public void ActiveCommandLine(bool value)
    {
        commandLineObject.SetActive(value);
    }
    #endregion
}
