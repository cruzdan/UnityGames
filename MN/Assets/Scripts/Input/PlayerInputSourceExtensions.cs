using UnityEngine;

public static class PlayerInputSourceExtensions
{
    public static IPlayerInputSource GetResolvedInput(
        this PlayerInputSource source)
    {
        MNManager mnManager = Object.FindAnyObjectByType<MNManager>();
        if (mnManager != null && mnManager.OverrideDeviceType)
        {
            return mnManager.IsAndroid
                ? source.Mobile
                : source.Keyboard;
        }

        if (source.IsOnPC())
            return source.Keyboard;

        if (source.IsOnMobile())
            return source.Mobile;

        return source.Keyboard;
    }
}
