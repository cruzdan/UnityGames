using UnityEngine;

public class DeviceChecker : MonoBehaviour
{
    #region Functions
    public bool IsOnPC()
    {
        NetworkGameManager networkGameManager = FindAnyObjectByType<NetworkGameManager>();
        if (networkGameManager.OverrideDeviceType)
            return !networkGameManager.IsAndroid;

        return Application.platform == RuntimePlatform.WindowsPlayer ||
                 Application.platform == RuntimePlatform.OSXPlayer ||
                 Application.platform == RuntimePlatform.LinuxPlayer;
    }

    public bool IsOnMobile()
    {
        NetworkGameManager networkGameManager = FindAnyObjectByType<NetworkGameManager>();
        if (networkGameManager.OverrideDeviceType)
            return networkGameManager.IsAndroid;

        return Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer;
    }
    #endregion
}
