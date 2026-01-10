using UnityEngine;

public class KeyboardInputSource : MonoBehaviour, IPlayerInputSource
{
    #region Functions
    public float GetHorizontalMovement()
    {
        return Input.GetAxis("Horizontal");
    }

    public float GetVerticalMovement()
    {
        return Input.GetAxis("Vertical");
    }

    public bool GetRun()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    public bool GetStopRun()
    {
        return Input.GetKeyUp(KeyCode.LeftShift);
    }

    public bool GetJump()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool GetCrouch()
    {
        return Input.GetKeyDown(KeyCode.LeftControl);
    }
    #endregion
}

