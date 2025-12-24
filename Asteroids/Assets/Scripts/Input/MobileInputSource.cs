using UnityEngine;

public class MobileInputSource : MonoBehaviour, IPlayerInputSource
{
    #region Serialized Fields
    [SerializeField] private HoldButton leftMovementButton;
    [SerializeField] private HoldButton rightMovementButton;
    [SerializeField] private HoldButton upMovementButton;
    [SerializeField] private HoldButton shootButton;
    #endregion
    #region Functions
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
        throw new System.NotImplementedException();
    }

    public bool GetRun()
    {
        throw new System.NotImplementedException();
    }

    public bool GetShoot()
    {
        return shootButton.IsPressed;
    }

    public float GetVerticalMovement()
    {
        return upMovementButton.IsPressed ? 1 : 0;
    }
    #endregion
}
