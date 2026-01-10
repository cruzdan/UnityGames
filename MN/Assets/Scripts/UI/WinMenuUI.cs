using System;
using UnityEngine;
using UnityEngine.UI;

public class WinMenuUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject winMenuObject;
    [SerializeField] private Exit exit;
    #endregion
    #region Actions
    public Action OnTryAgain;
    #endregion
    #region Public Properties
    public GameObject WinMenuObject => winMenuObject;
    #endregion
    #region Functions
    private void Awake()
    {
        tryAgainButton.onClick.AddListener(HandleTryAgain);
        exitButton.onClick.AddListener(HandleExit);
    }

    private void HandleTryAgain()
    {
        winMenuObject.SetActive(false);
        OnTryAgain?.Invoke();
    }

    private void HandleExit()
    {
        exit.ExitGame();
    }
    #endregion
}
