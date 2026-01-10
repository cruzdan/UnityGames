using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    #region UI
    [Header("UI")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;
    #endregion
    #region General
    [Header("General")]
    [SerializeField] private Exit exit;
    [SerializeField] private GameObject pauseMenuObject;
    #endregion
    #region Functions
    private void Awake()
    {
        continueButton.onClick.AddListener(HandleContinue);
        exitButton.onClick.AddListener(HandleExit);
    }

    private void HandleContinue()
    {
        Time.timeScale = 1f;
        pauseMenuObject.SetActive(false);
    }

    private void HandleExit()
    {
        exit.ExitGame();
    }
    #endregion
}
