using UnityEngine;

public class PauseManager : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winMenuObject;
    [SerializeField] private GameObject looseMenuObject;
    #endregion
    #region Private Variables
    private bool pause = false;
    #endregion
    #region Public Properties
    public bool IsPaused => pause;
    #endregion
    #region Functions
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (winMenuObject.activeSelf || looseMenuObject.activeSelf)
            {
                return;
            }
            Pause();
        }
    }

    public void Pause()
    {
        pause = !pause;
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        pauseMenu.SetActive(pause);
    }

    public void PauseWithoutUI()
    {
        Time.timeScale = 0f;
    }
    #endregion
}
