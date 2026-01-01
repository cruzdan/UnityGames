using UnityEngine;

public class PlayerUIActions : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Shoot shoot;
    [SerializeField] private PlayerPause playerPause;
    [SerializeField] private CommandsUI commandsUI;
    [SerializeField] private CheatsMenuUI cheatsMenuUI;
    #endregion
    #region Functions
    private void Awake()
    {
        commandsUI = FindAnyObjectByType<CommandsUI>();
        cheatsMenuUI = FindAnyObjectByType<CheatsMenuUI>();
        shoot.OnBulletNumberChanged += OnBulletNumberChanged;
        playerUI.OnPausePressed += OnPausePressed;
        playerPause.OnPausePressed += OnPausePressed;
    }

    void OnBulletNumberChanged(int newBulletNumber)
    {
        playerUI.SetBulletText(newBulletNumber.ToString());
    }

    void OnPausePressed()
    {
        if (playerPause.Pause)
        {
            playerUI.ClosePauseMenu();
        }
        else
        {
            playerUI.OpenPauseMenu();
            if (GameNetwork.Instance.IsOnline)
                commandsUI.ActiveCommandLine(false);
        }
        if (GameNetwork.Instance.IsOnline)
            commandsUI.ActiveCommandCanvas(playerPause.Pause);
        cheatsMenuUI.CloseCheatsMenu();
        playerPause.Pause = !playerPause.Pause;
    }
    #endregion
}
