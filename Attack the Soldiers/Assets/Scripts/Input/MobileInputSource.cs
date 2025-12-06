using UnityEngine;
using UnityEngine.UI;

public class MobileInputSource : MonoBehaviour, IPlayerInputSource
{
    #region Serialized Fields
    [SerializeField] private HoldButton leftMovementButton;
    [SerializeField] private HoldButton rightMovementButton;
    [SerializeField] private Button runLockButton;
    [SerializeField] private Image runLockImage;
    [SerializeField] private Color runLockActiveColor;
    [SerializeField] private Color runLockInactiveColor;
    [SerializeField] private HoldButton jumpButton;
    [SerializeField] private HoldButton shootButton;
    [SerializeField] private bool isRunLock;
    #endregion
    #region Functions
    private void Awake()
    {
        if (runLockButton != null)
        {
            runLockInactiveColor = runLockImage.color;
            runLockButton.onClick.AddListener(ToggleRunLock);
        }
    }
    public float GetHorizontalMovement()
    {
        if (rightMovementButton.IsPressed)
            return 1;
        if (leftMovementButton.IsPressed)
            return -1;
        return 0;
    }

    public bool GetJump()
    {
        return jumpButton.IsPressed;
    }

    public bool GetRun()
    {
        return isRunLock;
    }

    public bool GetShoot()
    {
        return shootButton.IsPressed;
    }

    void ToggleRunLock()
    {
        isRunLock = !isRunLock;
        if (isRunLock)
            runLockImage.color = runLockActiveColor;
        else
            runLockImage.color = runLockInactiveColor;
    }
    #endregion
}
