using UnityEngine;

public class CommandActions : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private CommandsUI commandsUI;
    [SerializeField] private CheatsMenuUI cheatsMenuUI;
    #endregion
    #region Functions
    void Start()
    {
        commandsUI.OnCommandSubmit += HandleCommand;
    }

    private void HandleCommand(string command)
    {
        switch (command)
        {
            case Constants.COMMAND_CHEATS:
                EnableCheats();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            EnableCheats();
        }
    }

    void EnableCheats()
    {
        cheatsMenuUI.OnCheatsOpen_Close();
    }
    #endregion
}
