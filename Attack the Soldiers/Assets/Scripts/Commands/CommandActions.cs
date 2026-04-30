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
        bool same = command.Equals(Constants.COMMAND_CHEATS, System.StringComparison.OrdinalIgnoreCase);
        switch (command)
        {
            case Constants.COMMAND_CHEATS:
                EnableCheats();
                break;
            default:
                break;
        }
    }

    void EnableCheats()
    {
        cheatsMenuUI.OnCheatsOpen_Close();
    }
    #endregion
}
