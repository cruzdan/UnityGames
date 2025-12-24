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

    public bool GetShoot()
    {
        return Input.GetButton("Fire1");
    }

    public bool GetRun()
    {
        throw new System.NotImplementedException();
    }

    public bool GetJump()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}

