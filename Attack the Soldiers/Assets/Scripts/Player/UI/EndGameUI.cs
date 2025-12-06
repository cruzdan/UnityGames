using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMPro.TMP_Text winnerText;
    [SerializeField] private Button exitButton;
    #endregion
    #region Functions
    private void Awake()
    {
        exitButton.onClick.AddListener(OnExitButtonPressed);
    }

    public void ShowEndGameMenu(int points)
    {
        endGamePanel.SetActive(true);
        // Si quieres mostrar al ganador:
        winnerText.text = "GAME OVER \nTotal Points: " + points;
    }

    void OnExitButtonPressed()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(0);
    }
    #endregion
}
