using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MobileInputSource : MonoBehaviour, IPlayerInputSource
{
    #region Serialized Fields
    [SerializeField] private HoldButton runButton;
    [SerializeField] private HoldButton jumpButton;
    [SerializeField] private HoldButton crouchButton;
    [SerializeField] private FloatingJoystick joystick;
    #endregion
    bool wasRunning = false;
    #region Functions
    public float GetHorizontalMovement()
    {
        return joystick.Horizontal;
    }

    public float GetVerticalMovement()
    {
        return joystick.Vertical;
    }

    public bool GetCrouch()
    {
        return crouchButton.ButtonDown;
    }

    public bool GetJump()
    {
        return jumpButton.ButtonDown;
    }

    public bool GetRun()
    {
        return runButton.ButtonDown;
    }

    public bool GetStopRun()
    {
        return runButton.ButtonUp;
    }
    #endregion
}
