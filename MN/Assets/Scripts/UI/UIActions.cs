using UnityEngine;

public class UIActions : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private WinMenuUI winMenuUI;
    [SerializeField] private LooseMenuUI looseMenuUI;
    [SerializeField] private MNManager manager;
    #endregion
    #region Functions
    private void Awake()
    {
        winMenuUI.OnTryAgain += HandleTryAgain;
        looseMenuUI.OnTryAgain += HandleTryAgain;
    }

    private void HandleTryAgain()
    {
        Time.timeScale = 1f;
        manager.Restart();
    }
    #endregion
}
