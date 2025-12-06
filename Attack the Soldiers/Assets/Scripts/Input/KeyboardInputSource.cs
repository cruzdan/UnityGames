using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInputSource : MonoBehaviour, IPlayerInputSource
{
    #region Serialized Fields
    [SerializeField] private PlayerInput playerInput;
    #endregion
    #region Functions
    public float GetHorizontalMovement()
    {
        if (playerInput.actions[Constants.INPUT_KEY_RIGHT].IsPressed())
            return 1;
        if (playerInput.actions[Constants.INPUT_KEY_LEFT].IsPressed())
            return -1;
        return 0;
    }

    public bool GetJump()
    {
        return playerInput.actions[Constants.INPUT_KEY_JUMP].IsPressed();
    }

    public bool GetRun()
    {
        return playerInput.actions[Constants.INPUT_KEY_RUN].IsPressed();
    }

    public bool GetShoot()
    {
        return playerInput.actions[Constants.INPUT_KEY_SHOOT].IsPressed();
    }
    #endregion
}

