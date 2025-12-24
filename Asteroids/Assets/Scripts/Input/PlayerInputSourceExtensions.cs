using UnityEngine;

public static class PlayerInputSourceExtensions
{
    public static IPlayerInputSource GetResolvedInput(
        this PlayerInputSource source)
    {
        AsteroidGameManager asteroidGamemanager = Object.FindObjectOfType<AsteroidGameManager>();
        if (asteroidGamemanager != null && asteroidGamemanager.OverrideDeviceType)
        {
            return asteroidGamemanager.IsAndroid
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
