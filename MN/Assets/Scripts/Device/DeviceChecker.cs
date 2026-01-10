using UnityEngine;

public class DeviceChecker : MonoBehaviour
{
    #region Functions
    public bool IsOnPC()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer ||
                 Application.platform == RuntimePlatform.OSXPlayer ||
                 Application.platform == RuntimePlatform.LinuxPlayer;
    }

    public bool IsOnMobile()
    {
        return Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer;
    }
    #endregion
}
